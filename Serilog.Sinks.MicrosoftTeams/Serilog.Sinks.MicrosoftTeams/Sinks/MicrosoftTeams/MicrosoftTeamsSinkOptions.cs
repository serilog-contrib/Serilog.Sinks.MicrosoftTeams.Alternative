// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkOptions.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Container for all Microsoft Teams sink configurations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams
{
    using System;

    using Serilog.Events;

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
        /// The default period.
        /// </summary>
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkOptions"/> class.
        /// </summary>
        /// <param name="webHookUri">The incoming web hook URI to the Microsoft Teams channel.</param>
        /// <param name="title">The title of messages.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch; defaults to 1 if
        /// not provided i.e. no batching by default.</param>
        /// <param name="period">The time to wait between checking for event batches; defaults to 1 sec if not
        /// provided.</param>
        /// <param name="formatProvider">The format provider used for formatting the message.</param>
        /// <param name="minimumLogEventLevel">The minimum log event level to use.</param>
        /// <param name="omitPropertiesSection">Indicates whether the properties section should be omitted or not.</param>
        public MicrosoftTeamsSinkOptions(string webHookUri, string title, int? batchSizeLimit = null, TimeSpan? period = null, IFormatProvider formatProvider = null, LogEventLevel minimumLogEventLevel = LogEventLevel.Verbose, bool omitPropertiesSection = false)
        {
            if (webHookUri == null)
            {
                throw new ArgumentNullException(nameof(webHookUri));
            }

            if (string.IsNullOrEmpty(webHookUri))
            {
                throw new ArgumentException(nameof(webHookUri));
            }

            this.WebHookUri = webHookUri;
            this.Title = title;
            this.BatchSizeLimit = batchSizeLimit ?? DefaultBatchSizeLimit;
            this.Period = period ?? DefaultPeriod;
            this.FormatProvider = formatProvider;
            this.MinimumLogEventLevel = minimumLogEventLevel;
            this.OmitPropertiesSection = omitPropertiesSection;
        }

        /// <summary>
        /// Gets the incoming web hook URI to the Microsoft Teams channel.
        /// </summary>
        public string WebHookUri { get; }

        /// <summary>
        /// Gets the title of messages.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the maximum number of events to post in a single batch.
        /// </summary>
        public int BatchSizeLimit { get; }

        /// <summary>
        /// Gets the time to wait between checking for event batches.
        /// </summary>
        public TimeSpan Period { get; }

        /// <summary>
        /// Gets the format provider used for formatting the message.
        /// </summary>
        public IFormatProvider FormatProvider { get; }

        /// <summary>
        /// Gets the minimum log event level.
        /// </summary>
        public LogEventLevel MinimumLogEventLevel { get; }

        /// <summary>
        /// Gets a value indicating whether the properties section should be omitted or not.
        /// </summary>
        public bool OmitPropertiesSection { get; }
    }
}