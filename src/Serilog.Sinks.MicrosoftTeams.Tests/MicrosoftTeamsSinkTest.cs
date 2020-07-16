// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkTest.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A test class to test the Microsoft Teams sink for basic functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Serilog.Debugging;
    using Serilog.Events;

    using Shouldly;

    using Xunit;

    /// <summary>
    /// A test class to test the Microsoft Teams sink for basic functionality.
    /// </summary>
    public class MicrosoftTeamsSinkTest
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkTest"/> class.
        /// </summary>
        public MicrosoftTeamsSinkTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));

            this.logger = TestHelper.CreateLogger();
        }

        /// <summary>
        /// Tests the emitting of events.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <param name="color">The color.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [Theory]
        [InlineData(LogEventLevel.Debug, "777777")]
        [InlineData(LogEventLevel.Error, "d9534f")]
        [InlineData(LogEventLevel.Fatal, "d9534f")]
        [InlineData(LogEventLevel.Information, "5bc0de")]
        [InlineData(LogEventLevel.Verbose, "777777")]
        [InlineData(LogEventLevel.Warning, "f0ad4e")]
        public async Task SinkEmitsEvents(LogEventLevel logEventLevel, string color)
        {
            const int MessageCount = 100;

            var sentMessagesTask = TestHelper.CaptureRequestsAsync(MessageCount);

            var renderedMessages = new List<string>();
            var templates = new List<string>();

            for (var i = 0; i < MessageCount; i++)
            {
                var templatePrefix = $"{Guid.NewGuid()} #";
                var template = templatePrefix + "{counter}";
                var renderedMessage = templatePrefix + i;

                renderedMessages.Add(renderedMessage);
                templates.Add(template);

                this.logger.Write(logEventLevel, template, i);
            }

            var actualMessages = await sentMessagesTask.ConfigureAwait(false);

            for (var i = 0; i < MessageCount; i++)
            {
                // ReSharper disable PossibleNullReferenceException
                var occuredOn = actualMessages[i]["sections"][0]["facts"].Last.Last.Last.ToString();
                var expectedMessage = TestHelper.CreateMessage(templates[i], renderedMessages[i], logEventLevel, color, i, occuredOn);
                actualMessages[i].ShouldBe(expectedMessage);
            }
        }
    }
}