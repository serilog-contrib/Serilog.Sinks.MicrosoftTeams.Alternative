using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Debugging;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    /// <summary>
    /// Test that adding static button(s) to messages are working
    /// </summary>
    public class MicrosoftTeamsSinkButtonTest
    {
        private ILogger _logger;

        private readonly List<MicrosoftTeamsSinkOptionsButton> _buttons = new List<MicrosoftTeamsSinkOptionsButton>
        {
            new MicrosoftTeamsSinkOptionsButton {Name = "FooBarBaz", Uri = "https://foobar.baz"},
            new MicrosoftTeamsSinkOptionsButton {Name = "FooBarBazBuzz", Uri = "https://foobar.baz.buzz"}
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkOmitPropertiesTest"/> class.
        /// </summary>
        public MicrosoftTeamsSinkButtonTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));
            this._logger = TestHelper.CreateLoggerWithButtons(_buttons);
        }

        /// <summary>
        /// Test that currect number of buttons are created on each message
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ShouldCreateCorrentNumberOfButtons(int buttonCount)
        {
            // Arrange
            this._logger = TestHelper.CreateLoggerWithButtons(_buttons.Take(buttonCount));

            // Act
            var sentMessagesTask = TestHelper.CaptureRequestsAsync(1);

            this._logger.Debug("Message text {prop}", 42);

            // Assert
            var actualMessages = await sentMessagesTask.ConfigureAwait(false);
            var actualMessage = actualMessages[0];
            var expectedMessage = buttonCount == 0 ? TestHelper.CreateMessage("Message text 42", "777777") : TestHelper.CreateMessageWithButton("Message text 42", "777777", _buttons.Take(buttonCount));
            actualMessage.ShouldBe(expectedMessage);
        }
    }
}
