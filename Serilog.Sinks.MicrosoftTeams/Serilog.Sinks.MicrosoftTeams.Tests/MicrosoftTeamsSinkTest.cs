using Serilog.Debugging;
using Serilog.Events;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    public class MicrosoftTeamsSinkTest
    {
        private readonly ILogger _logger;

        public MicrosoftTeamsSinkTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));

            _logger = TestHelper.CreateLogger();
        }

        [Theory]
        [InlineData(LogEventLevel.Debug, "777777")]
        [InlineData(LogEventLevel.Error, "d9534f")]
        [InlineData(LogEventLevel.Fatal, "d9534f")]
        [InlineData(LogEventLevel.Information, "5bc0de")]
        [InlineData(LogEventLevel.Verbose, "777777")]
        [InlineData(LogEventLevel.Warning, "f0ad4e")]
        public async Task SinkEmitsEvents(LogEventLevel logEventLevel, string color)
        {
            const int messageCount = 100;

            var sentMessagesTask = TestHelper.CaptureRequestsAsync(messageCount);

            var renderedMessages = new List<string>();
            var templates = new List<string>();

            for (var i = 0; i < messageCount; i++)
            {
                var templatePrefix = $"{Guid.NewGuid()} #";
                var template = templatePrefix + "{counter}";
                var renderedMessage = templatePrefix + i;

                renderedMessages.Add(renderedMessage);
                templates.Add(template);

                _logger.Write(logEventLevel, template, i);
            }

            var actualMessages = await sentMessagesTask.ConfigureAwait(false);

            for (var i = 0; i < messageCount; i++)
            {
                var expectedMessage = TestHelper.CreateMessage(templates[i], renderedMessages[i], logEventLevel,
                    color, i);
                actualMessages[i].ShouldBe(expectedMessage);
            }
        }
    }
}