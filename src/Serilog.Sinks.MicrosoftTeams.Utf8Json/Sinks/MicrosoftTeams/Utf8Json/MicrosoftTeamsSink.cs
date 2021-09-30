// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSink.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Implements <see cref="PeriodicBatchingSink" /> and provides means needed for sending Serilog log events to Microsoft Teams.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Utf8Json
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Serilog.Debugging;
    using Serilog.Events;
    using Serilog.Formatting.Display;
    using Serilog.Sinks.MicrosoftTeams.Alternative;
    using Serilog.Sinks.MicrosoftTeams.Alternative.Enumerations;
    using Serilog.Sinks.MicrosoftTeams.Alternative.Extensions;
    using Serilog.Sinks.MicrosoftTeams.Utf8Json.Core;
    using Serilog.Sinks.PeriodicBatching;

    using global::Utf8Json;
    using global::Utf8Json.Resolvers;

    /// <summary>
    /// Implements <see cref="PeriodicBatchingSink"/> and provides means needed for sending Serilog log events to Microsoft Teams.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class MicrosoftTeamsSink : PeriodicBatchingSink
    {
        /// <summary>
        /// The JSON formatter settings.
        /// </summary>
        private static readonly IJsonFormatterResolver JsonFormatterSettings = StandardResolver.AllowPrivateCamelCase;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// The options.
        /// </summary>
        private readonly MicrosoftTeamsSinkOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSink"/> class.
        /// </summary>
        /// <param name="options">Microsoft teams sink options object.</param>
        public MicrosoftTeamsSink(MicrosoftTeamsSinkOptions options) : base(options.BatchSizeLimit, options.Period, options.QueueLimit)
        {
            this.options = options;

            if (string.IsNullOrWhiteSpace(options.Proxy) == false)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new WebProxy(options.Proxy, true),
                    UseProxy = true
                };
                this.client = new HttpClient(httpClientHandler);
            }
            else
            {
                this.client = new HttpClient();
            }
        }

        /// <inheritdoc cref="PeriodicBatchingSink" />
        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        /// <returns></returns>
        /// <exception cref="LoggingFailedException">Received failed result {result.StatusCode} when posting events to Microsoft Teams</exception>
        /// <remarks>
        /// Override either <see cref="M:Serilog.Sinks.PeriodicBatching.PeriodicBatchingSink.EmitBatch(System.Collections.Generic.IEnumerable{Serilog.Events.LogEvent})" /> or <see cref="M:Serilog.Sinks.PeriodicBatching.PeriodicBatchingSink.EmitBatchAsync(System.Collections.Generic.IEnumerable{Serilog.Events.LogEvent})" />,
        /// not both. Overriding EmitBatch() is preferred.
        /// </remarks>
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var messages = this.GetMessagesToSend(events);
            await this.PostMessages(messages);
        }

        /// <summary>
        /// Free resources held by the sink.
        /// </summary>
        /// <param name="disposing">If true, called because the object is being disposed; if false,
        /// the object is being disposed from the finalizer.</param>
        /// <inheritdoc cref="PeriodicBatchingSink"/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.client.Dispose();
        }

        /// <summary>
        /// Gets the color of the attachment.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>The attachment color as <see cref="string"/>.</returns>
        private static string GetAttachmentColor(LogEventLevel level)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (level)
            {
                case LogEventLevel.Information:
                    return MicrosoftTeamsColors.Information;

                case LogEventLevel.Warning:
                    return MicrosoftTeamsColors.Warning;

                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    return MicrosoftTeamsColors.Error;

                default:
                    return MicrosoftTeamsColors.Default;
            }
        }

        /// <summary>
        /// Gets the messages to send and groups them.
        /// </summary>
        /// <param name="events">The log events.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="MicrosoftExtendedLogEvent"/>.</returns>
        private IEnumerable<MicrosoftExtendedLogEvent> GetMessagesToSend(IEnumerable<LogEvent> events)
        {
            var messagesToSend = new List<MicrosoftExtendedLogEvent>();

            foreach (var logEvent in events)
            {
                if (logEvent.Level < this.options.MinimumLogEventLevel)
                {
                    continue;
                }

                var foundSameLogEvent = messagesToSend.Where(m => m.LogEvent?.Exception?.Message != null).FirstOrDefault(l => l.LogEvent.Exception.Message == logEvent.Exception.Message);

                if (foundSameLogEvent is null)
                {
                    messagesToSend.Add(
                        new MicrosoftExtendedLogEvent
                        {
                            LogEvent = logEvent,
                            FirstOccurrence = logEvent.Timestamp,
                            LastOccurrence = logEvent.Timestamp
                        });
                }
                else
                {
                    if (foundSameLogEvent.FirstOccurrence > logEvent.Timestamp)
                    {
                        foundSameLogEvent.FirstOccurrence = logEvent.Timestamp;
                    }
                    else if (foundSameLogEvent.LastOccurrence < logEvent.Timestamp)
                    {
                        foundSameLogEvent.LastOccurrence = logEvent.Timestamp;
                    }
                }
            }

            return messagesToSend;
        }

        /// <summary>
        /// Posts the messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        private async Task PostMessages(IEnumerable<MicrosoftExtendedLogEvent> messages)
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var logEvent in messages)
            {
                try
                {
                    var message = this.CreateMessage(logEvent);
                    var json = JsonSerializer.ToJsonString(message, JsonFormatterSettings);
                    var result = await this.client.PostAsync(this.options.WebHookUri, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                    if (!result.IsSuccessStatusCode)
                    {
                        throw new LoggingFailedException($"Received failed result {result.StatusCode} when posting events to Microsoft Teams.");
                    }
                }
                catch (Exception ex)
                {
                    SelfLog.WriteLine($"{ex.Message} {ex.StackTrace}");
                    this.options.FailureCallback?.Invoke(ex);
                }
            }
        }

        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>A message card.</returns>
        private MicrosoftTeamsMessageCard CreateMessage(MicrosoftExtendedLogEvent logEvent)
        {
            var renderedMessage = this.GetRenderedMessage(logEvent);

            var request = new MicrosoftTeamsMessageCard
            {
                Title = this.GetRenderedTitle(logEvent),
                Text = this.options.UseCodeTagsForMessage ? $"```{Environment.NewLine}{renderedMessage}{Environment.NewLine}```" : renderedMessage,
                Color = GetAttachmentColor(logEvent.LogEvent.Level),
                Sections = this.options.OmitPropertiesSection ? null : new[]
                {
                    new MicrosoftTeamsMessageSection
                    {
                        Title = "Properties",
                        Facts = this.GetFacts(logEvent).ToArray()
                    }
                },
                PotentialActions = null
            };

            // Add static URL buttons from the options
            if (this.options.Buttons.IsNullOrEmpty())
            {
                return request;
            }

            request.PotentialActions = new List<MicrosoftTeamsMessageAction>();
            this.options.Buttons.ToList().ForEach(btn => request.PotentialActions.Add(new MicrosoftTeamsMessageAction("OpenUri", btn.Name, new MicrosoftTeamsMessageActionTargetUri(btn.Uri))));
            return request;
        }


        /// <summary>
        /// Gets the rendered message from the <see cref="MicrosoftExtendedLogEvent"/>.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>The rendered messages as <see cref="string"/>.</returns>
        private string GetRenderedMessage(MicrosoftExtendedLogEvent logEvent)
        {
            if (string.IsNullOrWhiteSpace(this.options.OutputTemplate))
            {
                return logEvent.LogEvent.RenderMessage(this.options.FormatProvider);
            }

            var formatter = new MessageTemplateTextFormatter(this.options.OutputTemplate, this.options.FormatProvider);
            var stringWriter = new StringWriter();
            formatter.Format(logEvent.LogEvent, stringWriter);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Gets the rendered title from the <see cref="MicrosoftExtendedLogEvent"/>.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>The rendered messages as <see cref="string"/>.</returns>
        private string GetRenderedTitle(MicrosoftExtendedLogEvent logEvent)
        {
            if (string.IsNullOrWhiteSpace(this.options.TitleTemplate))
            {
                return logEvent.LogEvent.RenderMessage(this.options.FormatProvider);
            }

            var formatter = new MessageTemplateTextFormatter(this.options.TitleTemplate, this.options.FormatProvider);
            var stringWriter = new StringWriter();
            formatter.Format(logEvent.LogEvent, stringWriter);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Gets the facts.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>A list of facts.</returns>
        private IEnumerable<MicrosoftTeamsMessageFact> GetFacts(MicrosoftExtendedLogEvent logEvent)
        {
            yield return new MicrosoftTeamsMessageFact
            {
                Name = "Level",
                Value = logEvent.LogEvent.Level.ToString()
            };

            yield return new MicrosoftTeamsMessageFact
            {
                Name = "MessageTemplate",
                Value = this.options.UseCodeTagsForMessage ? $"```{Environment.NewLine}{logEvent.LogEvent.MessageTemplate.Text}{Environment.NewLine}```" : logEvent.LogEvent.MessageTemplate.Text
            };

            if (logEvent.LogEvent.Exception != null)
            {
                yield return new MicrosoftTeamsMessageFact { Name = "Exception", Value = logEvent.LogEvent.Exception.ToString() };
            }

            foreach (var property in logEvent.LogEvent.Properties)
            {
                yield return new MicrosoftTeamsMessageFact
                {
                    Name = property.Key,
                    Value = property.Value.ToString(null, this.options.FormatProvider)
                };
            }

            if (logEvent.FirstOccurrence != logEvent.LastOccurrence)
            {
                yield return new MicrosoftTeamsMessageFact
                {
                    Name = "First occurrence",
                    Value = logEvent.FirstOccurrence.ToString("dd.MM.yyyy HH:mm:sszzz", this.options.FormatProvider)
                };

                yield return new MicrosoftTeamsMessageFact
                {
                    Name = "Last occurrence",
                    Value = logEvent.LastOccurrence.ToString("dd.MM.yyyy HH:mm:sszzz", this.options.FormatProvider)
                };
            }
            else
            {
                yield return new MicrosoftTeamsMessageFact
                {
                    Name = "Occurred on",
                    Value = logEvent.FirstOccurrence.ToString("dd.MM.yyyy HH:mm:sszzz", this.options.FormatProvider)
                };
            }
        }
    }
}