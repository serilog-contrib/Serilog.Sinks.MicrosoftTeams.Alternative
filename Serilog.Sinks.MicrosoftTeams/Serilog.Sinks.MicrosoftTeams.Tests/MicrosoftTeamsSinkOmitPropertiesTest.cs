// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkOmitPropertiesTest.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A test class to test the omitting of properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Tests
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Serilog.Debugging;

    using Shouldly;

    using Xunit;

    /// <summary>
    /// A test class to test the omitting of properties.
    /// </summary>
    public class MicrosoftTeamsSinkOmitPropertiesTest
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkOmitPropertiesTest"/> class.
        /// </summary>
        public MicrosoftTeamsSinkOmitPropertiesTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));
            this.logger = TestHelper.CreateLogger(true);
        }

        /// <summary>
        /// Test the omit properties feature.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [Fact]
        public async Task ShouldOmitProperties()
        {
            var sentMessagesTask = TestHelper.CaptureRequestsAsync(1);

            this.logger.Debug("Message text {prop}", 42);

            var actualMessages = await sentMessagesTask.ConfigureAwait(false);
            var actualMessage = actualMessages[0];
            var expectedMessage = TestHelper.CreateMessage("Message text 42", "777777");
            actualMessage.ShouldBe(expectedMessage);
        }
    }
}