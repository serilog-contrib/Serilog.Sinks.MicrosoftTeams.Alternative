using Serilog.Debugging;
using Shouldly;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    public class MicrosoftTeamsSinkOmitPropertiesTest
    {
        private readonly ILogger _logger;

        public MicrosoftTeamsSinkOmitPropertiesTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));

            _logger = TestHelper.CreateLogger(omitPropertiesSection: true);
        }

        [Fact]
        public async Task ShouldOmitProperties()
        {
            var sentMessagesTask = TestHelper.CaptureRequestsAsync(1);

            _logger.Debug("Message text {prop}", 42);

            var actualMessages = await sentMessagesTask.ConfigureAwait(false);
            var actualMessage = actualMessages[0];
            var expectedMessage = TestHelper.CreateMessage("Message text 42", "777777");
            actualMessage.ShouldBe(expectedMessage);
        }
    }
}