using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.MicrosoftTeams;
using System;

namespace Serilog
{
    /// <summary>
    /// Provides extension methods on <see cref="LoggerSinkConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationMicrosoftTeamsExtensions
    {
        /// <summary>
        /// <see cref="LoggerSinkConfiguration"/> extension that provides configuration chaining.
        /// <example>
        ///     new LoggerConfiguration()
        ///         .MinimumLevel.Verbose()
        ///         .WriteTo.MicrosoftTeams("webHookUri")
        ///         .CreateLogger();
        /// </example>
        /// </summary>
        /// <param name="loggerSinkConfiguration">Instance of <see cref="LoggerSinkConfiguration"/> object.</param>
        /// <param name="webHookUri">The incoming webhook URI to the Microsoft Teams channel.</param>
        /// <param name="title">The title of messages.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch; defaults to 1 if
        /// not provided i.e. no batching by default.</param>
        /// <param name="period">The time to wait between checking for event batches; defaults to 1 sec if not
        /// provided.</param>
        /// <param name="formatProvider">The format provider used for formatting the message.</param>
        /// <param name="restrictedToMinimumLevel"><see cref="LogEventLevel"/> value that specifies minimum logging
        /// level that will be allowed to be logged.</param>
        /// <returns>Instance of <see cref="LoggerConfiguration"/> object.</returns>
        public static LoggerConfiguration MicrosoftTeams(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            string webHookUri,
            string title = null,
            int? batchSizeLimit = null,
            TimeSpan? period = null,
            IFormatProvider formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            var microsoftTeamsSinkOptions = new MicrosoftTeamsSinkOptions(webHookUri, title, batchSizeLimit, period,
                formatProvider);

            return loggerSinkConfiguration.MicrosoftTeams(microsoftTeamsSinkOptions, restrictedToMinimumLevel);
        }

        /// <summary>
        /// <see cref="LoggerSinkConfiguration"/> extension that provides configuration chaining.
        /// </summary>
        /// <param name="loggerSinkConfiguration">Instance of <see cref="LoggerSinkConfiguration"/> object.</param>
        /// <param name="microsoftTeamsSinkOptions">The microsoft teams sink options object.</param>
        /// <param name="restrictedToMinimumLevel"><see cref="LogEventLevel"/> value that specifies minimum logging
        /// level that will be allowed to be logged.</param>
        /// <returns>Instance of <see cref="LoggerConfiguration"/> object.</returns>
        public static LoggerConfiguration MicrosoftTeams(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            MicrosoftTeamsSinkOptions microsoftTeamsSinkOptions,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerSinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerSinkConfiguration));
            }

            if (microsoftTeamsSinkOptions == null)
            {
                throw new ArgumentNullException(nameof(microsoftTeamsSinkOptions));
            }

            if (string.IsNullOrWhiteSpace(microsoftTeamsSinkOptions.WebHookUri))
            {
                throw new ArgumentNullException(nameof(microsoftTeamsSinkOptions.WebHookUri));
            }

            return loggerSinkConfiguration.Sink(new MicrosoftTeamsSink(microsoftTeamsSinkOptions), restrictedToMinimumLevel);
        }
    }
}