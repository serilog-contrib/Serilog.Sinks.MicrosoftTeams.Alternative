// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkOptions.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Container for all Microsoft Teams sink configurations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative;

/// <summary>
/// Container for all Microsoft Teams sink configurations.
/// </summary>
public class MicrosoftTeamsSinkOptions
{
    /// <summary>
    /// The default batch size limit.
    /// </summary>
    private const int DefaultBatchSizeLimit = 1;

    /// <summary>
    /// The default queue limit.
    /// </summary>
    private const int DefaultQueueLimit = int.MaxValue;

    /// <summary>
    /// The default period.
    /// </summary>
    private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkOptions"/> class.
    /// </summary>
    /// <param name="webHookUri">The incoming web hook URI to the Microsoft Teams channel.</param>
    /// <param name="titleTemplate">The title template of the messages.</param>
    /// <param name="batchSizeLimit">The maximum number of events to post in a single batch; defaults to 1 if
    /// not provided i.e. no batching by default.</param>
    /// <param name="period">The time to wait between checking for event batches; defaults to 1 sec if not
    /// provided.</param>
    /// <param name="outputTemplate">The output template.</param>
    /// <param name="formatProvider">The format provider used for formatting the message.</param>
    /// <param name="minimumLogEventLevel">The minimum log event level to use.</param>
    /// <param name="omitPropertiesSection">A value indicating whether the properties section should be omitted or not.</param>
    /// <param name="useCodeTagsForMessage">A value indicating whether code tags are used for the message template or not.</param>
    /// <param name="usePowerAutomateWorkflows">A value indicating whether Power Automate workflows are used or not.</param>
    /// <param name="proxy">The proxy address to use.</param>
    /// <param name="buttons">The buttons to add to a message.</param>
    /// <param name="queueLimit">The maximum number of events that should be stored in the batching queue.</param>
    /// <param name="channelHandler">The configuration for sending events to multiple channels.</param>
    public MicrosoftTeamsSinkOptions(
        string webHookUri,
        string? titleTemplate = null,
        int? batchSizeLimit = null,
        TimeSpan? period = null,
        string? outputTemplate = null,
        IFormatProvider? formatProvider = null,
        LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose,
        bool omitPropertiesSection = false,
        bool useCodeTagsForMessage = false,
        bool usePowerAutomateWorkflows = false,
        string? proxy = null,
        IEnumerable<MicrosoftTeamsSinkOptionsButton>? buttons = null,
        int? queueLimit = null,
        MicrosoftTeamsSinkChannelHandlerOptions? channelHandler = null)
    {
        if (string.IsNullOrWhiteSpace(webHookUri))
        {
            throw new ArgumentException("The webhook URI is empty.", nameof(webHookUri));
        }

        this.WebHookUri = webHookUri;
        this.TitleTemplate = titleTemplate;
        this.BatchSizeLimit = batchSizeLimit ?? DefaultBatchSizeLimit;
        this.Period = period ?? DefaultPeriod;
        this.OutputTemplate = outputTemplate;
        this.FormatProvider = formatProvider;
        this.MinimumLogEventLevel = minimumLogEventLevel;
        this.OmitPropertiesSection = omitPropertiesSection;
        this.UseCodeTagsForMessage = useCodeTagsForMessage;
        this.UsePowerAutomateWorkflows = usePowerAutomateWorkflows;
        this.Proxy = proxy;
        this.Buttons = buttons ?? [];
        this.QueueLimit = queueLimit ?? DefaultQueueLimit;
        this.ChannelHandler = channelHandler ?? new MicrosoftTeamsSinkChannelHandlerOptions();
    }

    /// <summary>
    /// Gets the incoming web hook URI to the Microsoft Teams channel.
    /// </summary>
    public string WebHookUri { get; } = string.Empty;

    /// <summary>
    /// Gets the title template of messages.
    /// </summary>
    public string? TitleTemplate { get; }

    /// <summary>
    /// Gets the maximum number of events to post in a single batch.
    /// </summary>
    public int BatchSizeLimit { get; }

    /// <summary>
    /// Gets the time to wait between checking for event batches.
    /// </summary>
    public TimeSpan Period { get; }

    /// <summary>
    /// Gets the output template.
    /// </summary>
    public string? OutputTemplate { get; }

    /// <summary>
    /// Gets the format provider used for formatting the message.
    /// </summary>
    public IFormatProvider? FormatProvider { get; }

    /// <summary>
    /// Gets the minimum log event level.
    /// </summary>
    public LogEventLevel MinimumLogEventLevel { get; }

    /// <summary>
    /// Gets a value indicating whether the properties section should be omitted or not.
    /// </summary>
    public bool OmitPropertiesSection { get; }

    /// <summary>
    /// Gets a value indicating whether code tags are used for the message template or not.
    /// </summary>
    public bool UseCodeTagsForMessage { get; }

    /// <summary>
    /// A value indicating whether Power Automate workflows are used or not (since O365 Incoming Webhooks are being shutdown).
    /// </summary>
    public bool UsePowerAutomateWorkflows { get; }

    /// <summary>
    /// Gets the proxy URL.
    /// </summary>
    public string? Proxy { get; }

    /// <summary>
    /// Gets the buttons to add to a message.
    /// </summary>
    public IEnumerable<MicrosoftTeamsSinkOptionsButton>? Buttons { get; }

    /// <summary>
    /// Gets the maximum number of events that should be stored in the batching queue.
    /// </summary>
    public int QueueLimit { get; }

    /// <summary>
    /// Gets the configuration for sending events to multiple channels.
    /// </summary>
    public MicrosoftTeamsSinkChannelHandlerOptions ChannelHandler { get; }
}
