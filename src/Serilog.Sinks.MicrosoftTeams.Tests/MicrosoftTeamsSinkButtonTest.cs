// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkButtonTest.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A test class to test the adding of static buttons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Serilog.Debugging;

    using Shouldly;

    using Xunit;

    /// <summary>
    /// A test class to test the adding of static buttons.
    /// </summary>
    public class MicrosoftTeamsSinkButtonTest
    {
        /// <summary>
        /// The buttons.
        /// </summary>
        private readonly List<MicrosoftTeamsSinkOptionsButton> buttons = new List<MicrosoftTeamsSinkOptionsButton>
        {
            new MicrosoftTeamsSinkOptionsButton { Name = "FooBarBaz", Uri = "https://foobar.baz" },
            new MicrosoftTeamsSinkOptionsButton { Name = "FooBarBazBuzz", Uri = "https://foobar.baz.buzz" }
        };

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkButtonTest"/> class.
        /// </summary>
        public MicrosoftTeamsSinkButtonTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));
            this.logger = TestHelper.CreateLoggerWithButtons(this.buttons);
        }

        /// <summary>
        /// Test that correct number of buttons are created on each message.
        /// </summary>
        /// <param name="buttonCount">The button count.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ShouldCreateCorrectNumberOfButtons(int buttonCount)
        {
            // Arrange
            this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(buttonCount));

            // Act
            var sentMessagesTask = TestHelper.CaptureRequestsAsync(1);

            this.logger.Debug("Message text {prop}", 42);

            // Assert
            var actualMessages = await sentMessagesTask.ConfigureAwait(false);
            var actualMessage = actualMessages[0];
            var expectedMessage = buttonCount == 0 ? TestHelper.CreateMessage("Message text 42", "777777") : TestHelper.CreateMessageWithButton("Message text 42", "777777", this.buttons.Take(buttonCount));
            actualMessage.ShouldBe(expectedMessage);
        }
    }
}
