using Newtonsoft.Json;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// Implements <see cref="PeriodicBatchingSink"/> and provides means needed for sending Serilog log events to Microsoft Teams.
    /// </summary>
    public class MicrosoftTeamsSink : PeriodicBatchingSink
    {
        /// <summary>
        /// The client.
        /// </summary>
        private static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// The json serializer settings.
        /// </summary>
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// The options.
        /// </summary>
        private readonly MicrosoftTeamsSinkOptions _options;

        /// <summary>
        /// Initializes new instance of <see cref="MicrosoftTeamsSink"/>.
        /// </summary>
        /// <param name="options">Microsoft teams sink options object.</param>
        public MicrosoftTeamsSink(MicrosoftTeamsSinkOptions options)
                : base(options.BatchSizeLimit, options.Period)
        {
            _options = options;
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
            var messagesToSend = new List<ExtendedLogEvent>();

            foreach (var logEvent in events)
            {
                if (logEvent.Level < _options.MinimumLogEventLevel)
                {
                    continue;
                }

                var foundSameLogEvent = messagesToSend.FirstOrDefault(l => l.LogEvent.Exception.Message == logEvent.Exception.Message);

                if (foundSameLogEvent == null)
                {
                    messagesToSend.Add(
                        new ExtendedLogEvent
                            {
                                LogEvent = logEvent,
                                FirstOccurance = logEvent.Timestamp,
                                LastOccurance = logEvent.Timestamp
                            });
                }
                else
                {
                    if (foundSameLogEvent.FirstOccurance > logEvent.Timestamp)
                    {
                        foundSameLogEvent.FirstOccurance = logEvent.Timestamp;
                    }
                    else if (foundSameLogEvent.LastOccurance < logEvent.Timestamp)
                    {
                        foundSameLogEvent.LastOccurance = logEvent.Timestamp;
                    }
                }
            }

            foreach (var logEvent in messagesToSend)
            {
                var message = CreateMessage(logEvent);
                var json = JsonConvert.SerializeObject(message, JsonSerializerSettings);
                var result = await Client.PostAsync(_options.WebHookUri, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                if (!result.IsSuccessStatusCode)
                {
                    throw new LoggingFailedException($"Received failed result {result.StatusCode} when posting events to Microsoft Teams");
                }
            }
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
            Client.Dispose();
        }

        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>A message card.</returns>
        private MicrosoftTeamsMessageCard CreateMessage(ExtendedLogEvent logEvent)
        {
            var renderedMessage = logEvent.LogEvent.RenderMessage(_options.FormatProvider);

            var request = new MicrosoftTeamsMessageCard
            {
                Title = _options.Title,
                Text = renderedMessage,
                Color = GetAttachmentColor(logEvent.LogEvent.Level),
                Sections = _options.OmitPropertiesSection ? null : new[]
                {
                    new MicrosoftTeamsMessageSection
                    {
                        Title = "Properties",
                        Facts = GetFacts(logEvent).ToArray()
                    }
                }
            };

            return request;
        }

        /// <summary>
        /// Gets the facts.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns>A list of facts.</returns>
        private IEnumerable<MicrosoftTeamsMessageFact> GetFacts(ExtendedLogEvent logEvent)
        {
            yield return new MicrosoftTeamsMessageFact
            {
                Name = "Level",
                Value = logEvent.LogEvent.Level.ToString()
            };

            yield return new MicrosoftTeamsMessageFact
            {
                Name = "MessageTemplate",
                Value = logEvent.LogEvent.MessageTemplate.Text
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
                    Value = property.Value.ToString(null, _options.FormatProvider)
                };
            }

            if (logEvent.FirstOccurance != logEvent.LastOccurance)
            {
                yield return new MicrosoftTeamsMessageFact
                                 {
                                     Name = "First occurence",
                                     Value = logEvent.FirstOccurance.ToString(
                                         "dd.MM.yyyy HH:mm:sszzz",
                                         _options.FormatProvider)
                                 };

                yield return new MicrosoftTeamsMessageFact
                                 {
                                     Name = "Last occurance",
                                     Value = logEvent.LastOccurance.ToString(
                                         "dd.MM.yyyy HH:mm:sszzz",
                                         _options.FormatProvider)
                                 };
            }
            else
            {
                yield return new MicrosoftTeamsMessageFact
                                 {
                                     Name = "Occured on",
                                     Value = logEvent.FirstOccurance.ToString(
                                         "dd.MM.yyyy HH:mm:sszzz",
                                         _options.FormatProvider)
                                 };
            }
        }

        /// <summary>
        /// Gets the color of the attachment.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>The attachement color as <see cref="string"/>.</returns>
        private static string GetAttachmentColor(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Information:
                    return "5bc0de";

                case LogEventLevel.Warning:
                    return "f0ad4e";

                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    return "d9534f";

                default:
                    return "777777";
            }
        }
    }
}