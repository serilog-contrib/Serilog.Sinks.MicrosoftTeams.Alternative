// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A helper class for the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A helper class for the tests.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class TestHelper
    {
        /// <summary>
        /// The test web hook URL.
        /// </summary>
        private static readonly string TestWebHook = Environment.GetEnvironmentVariable("MicrosoftTeamsWebhookUrl");

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="omitPropertiesSection">A value indicating whether the properties should be omitted or not.</param>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLogger(bool omitPropertiesSection = false)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", omitPropertiesSection: omitPropertiesSection))
                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Creates the logger with buttons.
        /// </summary>
        /// <param name="buttons">´The buttons to output</param>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLoggerWithButtons(IEnumerable<MicrosoftTeamsSinkOptionsButton> buttons)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", buttons: buttons))
                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <returns>An <see cref="ILogger"/>.</returns>
        public static ILogger CreateLoggerWithCodeTags()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", useCodeTagsForMessage: true))
                .CreateLogger();

            return logger;
        }
    }
}