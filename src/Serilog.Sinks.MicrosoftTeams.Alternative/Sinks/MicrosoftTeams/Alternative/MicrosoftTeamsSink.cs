// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSink.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Implements <see cref="IBatchedLogEventSink" /> and provides means needed for sending Serilog log events to Microsoft Teams.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative;

/// <summary>
/// Implements <see cref="IBatchedLogEventSink"/> and provides means needed for sending Serilog log events to Microsoft Teams.
/// </summary>
public class MicrosoftTeamsSink : IBatchedLogEventSink
{
    /// <summary>
    /// The json serializer settings.
    /// </summary>
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

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
    public MicrosoftTeamsSink(MicrosoftTeamsSinkOptions options)
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

    /// <inheritdoc cref="IBatchedLogEventSink" />
    /// <summary>
    /// Emit a batch of log events, running asynchronously.
    /// </summary>
    /// <param name="events">The events to emit.</param>
    /// <returns></returns>
    /// <exception cref="LoggingFailedException">Received failed result {result.StatusCode} when posting events to Microsoft Teams</exception>
    /// <remarks>
    /// Override either <see cref="M:Serilog.Sinks.PeriodicBatching.IBatchedLogEventSink.EmitBatch(System.Collections.Generic.IEnumerable{Serilog.Events.LogEvent})" /> or <see cref="M:Serilog.Sinks.PeriodicBatching.IBatchedLogEventSink.EmitBatchAsync(System.Collections.Generic.IEnumerable{Serilog.Events.LogEvent})" />,
    /// not both. Overriding EmitBatch() is preferred.
    /// </remarks>
    public async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
        var messages = this.GetMessagesToSend(events);
        await this.PostMessages(messages);
    }

    /// <inheritdoc cref="IBatchedLogEventSink" />
    /// <summary>
    /// Allows sinks to perform periodic work without requiring additional threads or
    /// timers (thus avoiding additional flush/shut-down complexity).   
    /// </summary>
    public Task OnEmptyBatchAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the color of the attachment.
    /// </summary>
    /// <param name="level">The level.</param>
    /// <returns>The attachment color as <see cref="string"/>.</returns>
    private static string GetAttachmentColor(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Information => MicrosoftTeamsColors.Information,
            LogEventLevel.Warning => MicrosoftTeamsColors.Warning,
            LogEventLevel.Error or LogEventLevel.Fatal => MicrosoftTeamsColors.Error,
            _ => MicrosoftTeamsColors.Default,
        };
    }

    /// <summary>
    /// Gets the messages to send and groups them.
    /// </summary>
    /// <param name="events">The log events.</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="MicrosoftExtendedLogEvent"/>.</returns>
    private IEnumerable<MicrosoftExtendedLogEvent> GetMessagesToSend(IEnumerable<LogEvent> events)
    {
        var isFilterEnabled = this.options.ChannelHandler.IsFilterOnPropertyEnabled;
        var messagesToSend = new List<MicrosoftExtendedLogEvent>();

        foreach (var logEvent in events)
        {
            if (logEvent.Level < this.options.MinimumLogEventLevel)
            {
                continue;
            }

            // Ignore messages that do not comply with the filter.
            if (isFilterEnabled && !logEvent.Properties.ContainsKey(this.options.ChannelHandler.FilterOnProperty))
            {
                continue;
            }

            var foundSameLogEvent = messagesToSend.Where(m => m.LogEvent?.Exception?.Message != null).FirstOrDefault(l => l.LogEvent.Exception.Message == logEvent.Exception.Message);

            if (foundSameLogEvent is null)
            {
                messagesToSend.Add(new MicrosoftExtendedLogEvent(logEvent.Timestamp.DateTime, logEvent.Timestamp.DateTime, logEvent));
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
        var isFilterEnabled = this.options.ChannelHandler.IsFilterOnPropertyEnabled;

        foreach (var logEvent in messages)
        {
            try
            {
                var webHookUri = isFilterEnabled
                    ? this.GetChannelUri(logEvent)
                    : this.options.WebHookUri;

                var message = this.CreateMessage(logEvent);
                var json = JsonConvert.SerializeObject(message, JsonSerializerSettings);
                var result = await this.client.PostAsync(webHookUri, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);

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
    ///     If support for multiple channels is enabled this will get
    ///     the corresponding Uri for the channel or the default Uri if
    ///     there is no specific configuration for the event.
    /// </summary>
    /// <param name="logEvent">Event to check for the target channel.</param>
    /// <returns>Uri for the target channel.</returns>
    private string GetChannelUri(MicrosoftExtendedLogEvent logEvent)
    {
        var expectedKey = logEvent.LogEvent
            .Properties[this.options.ChannelHandler.FilterOnProperty]
            .ToString()
            .Trim('"');

        // Return the specific channel uri if available.
        if (this.options.ChannelHandler.ChannelList.ContainsKey(expectedKey))
        {
            return this.options.ChannelHandler.ChannelList[expectedKey];
        }

        return this.options.WebHookUri;
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
            Sections = this.options.OmitPropertiesSection ? new List<MicrosoftTeamsMessageSection>() : new[]
            {
                    new MicrosoftTeamsMessageSection
                    {
                        Title = "Properties",
                        Facts = this.GetFacts(logEvent).ToArray()
                    }
                },
            PotentialActions = new List<MicrosoftTeamsMessageAction>()
        };

        // Add static URL buttons from the options
        if (this.options.Buttons.IsNullOrEmpty())
        {
            return request;
        }

        request.PotentialActions = new List<MicrosoftTeamsMessageAction>();
        this.options.Buttons!.ToList().ForEach(btn => request.PotentialActions.Add(new MicrosoftTeamsMessageAction("OpenUri", btn.Name, new MicrosoftTeamsMessageActionTargetUri(btn.Uri))));
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
