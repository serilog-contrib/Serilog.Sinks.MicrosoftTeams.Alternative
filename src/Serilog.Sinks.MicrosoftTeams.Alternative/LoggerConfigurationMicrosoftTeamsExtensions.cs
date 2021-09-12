// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerConfigurationMicrosoftTeamsExtensions.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   Provides extension methods on <see cref="LoggerSinkConfiguration" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog
{
    using System;
    using System.Collections.Generic;

    using Serilog.Configuration;
    using Serilog.Events;
    using Serilog.Sinks.MicrosoftTeams.Alternative;

    /// <summary>
    /// Provides extension methods on <see cref="LoggerSinkConfiguration"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
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
        /// <param name="webHookUri">The incoming web hook URI to the Microsoft Teams channel.</param>
        /// <param name="titleTemplate">The title template of the messages.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch; defaults to 1 if
        /// not provided i.e. no batching by default.</param>
        /// <param name="period">The time to wait between checking for event batches; defaults to 1 sec if not
        /// provided.</param>
        /// <param name="outputTemplate">The output template.</param>
        /// <param name="formatProvider">The format provider used for formatting the message.</param>
        /// <param name="restrictedToMinimumLevel"><see cref="LogEventLevel"/> value that specifies minimum logging
        /// level that will be allowed to be logged.</param>
        /// <param name = "proxy" > The proxy address to use.</param>
        /// <param name="omitPropertiesSection">Indicates whether the properties section should be omitted or not.</param>
        /// <param name="useCodeTagsForMessage">A value indicating whether code tags are used for the message template or not.</param>
        /// <param name="buttons">The buttons to add to a message.</param>
        /// <param name="failureCallback">The failure callback.</param>
        /// <param name="queueLimit">The maximum number of events that should be stored in the batching queue.</param>
        /// <returns>Instance of <see cref="LoggerConfiguration"/> object.</returns>
        // ReSharper disable once InconsistentNaming
        public static LoggerConfiguration MicrosoftTeams(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            string webHookUri,
            string titleTemplate = null,
            int? batchSizeLimit = null,
            TimeSpan? period = null,
            string outputTemplate = null,
            IFormatProvider formatProvider = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string proxy = null,
            bool omitPropertiesSection = false,
            bool useCodeTagsForMessage = false,
            IEnumerable<MicrosoftTeamsSinkOptionsButton> buttons = null,
            Action<Exception> failureCallback = null,
            int? queueLimit = null)
        {
            var microsoftTeamsSinkOptions = new MicrosoftTeamsSinkOptions(
                webHookUri,
                titleTemplate,
                batchSizeLimit,
                period,
                outputTemplate,
                formatProvider,
                restrictedToMinimumLevel,
                omitPropertiesSection,
                useCodeTagsForMessage,
                proxy,
                buttons,
                failureCallback,
                queueLimit);
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
        // ReSharper disable once InconsistentNaming
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