using System;

namespace Serilog.Sinks.MicrosoftTeams
{
    /// <summary>
    /// Container for all Microsoft Teams sink configurations.
    /// </summary>
    public class MicrosoftTeamsSinkOptions
    {
        /// <summary>
        /// The default period.
        /// </summary>
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The default batch size limit.
        /// </summary>
        private const int DefaultBatchSizeLimit = 1;

        /// <summary>
        /// Create an instance of the Microsoft Teams options container.
        /// </summary>
        /// <param name="webHookUri">The incoming webhook URI to the Microsoft Teams channel.</param>
        /// <param name="title">The title of messages.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch; defaults to 1 if
        /// not provided i.e. no batching by default.</param>
        /// <param name="period">The time to wait between checking for event batches; defaults to 1 sec if not
        /// provided.</param>
        /// <param name="formatProvider">The format provider used for formatting the message.</param>
        public MicrosoftTeamsSinkOptions(string webHookUri, string title, int? batchSizeLimit = null,
            TimeSpan? period = null, IFormatProvider formatProvider = null)
        {
            if (webHookUri == null)
            {
                throw new ArgumentNullException(nameof(webHookUri));
            }

            if (string.IsNullOrEmpty(webHookUri))
            {
                throw new ArgumentException(nameof(webHookUri));
            }

            WebHookUri = webHookUri;
            Title = title;
            BatchSizeLimit = batchSizeLimit ?? DefaultBatchSizeLimit;
            Period = period ?? DefaultPeriod;
            FormatProvider = formatProvider;
        }

        /// <summary>
        /// The incoming webhook URI to the Microsoft Teams channel.
        /// </summary>
        public string WebHookUri { get; }

        /// <summary>
        /// The title of messages.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The maximum number of events to post in a single batch.
        /// </summary>
        public int BatchSizeLimit { get; }

        /// <summary>
        /// The time to wait between checking for event batches.
        /// </summary>
        public TimeSpan Period { get; }

        /// <summary>
        /// The format provider used for formatting the message.
        /// </summary>
        public IFormatProvider FormatProvider { get; }
    }
}