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
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Serilog.Debugging;
    using Serilog.Events;

    /// <summary>
    /// A test class to test the Microsoft Teams sink for basic functionality.
    /// </summary>
    [TestClass]
    public class MicrosoftTeamsSinkTest
    {
        /// <summary>
        /// The buttons.
        /// </summary>
        private readonly List<MicrosoftTeamsSinkOptionsButton> buttons = new List<MicrosoftTeamsSinkOptionsButton>
        {
            new MicrosoftTeamsSinkOptionsButton { Name = "Google", Uri = "https://google.com" },
            new MicrosoftTeamsSinkOptionsButton { Name = "DuckDuckGo", Uri = "https://duckduckgo.com" }
        };

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTeamsSinkTest"/> class.
        /// </summary>
        public MicrosoftTeamsSinkTest()
        {
            SelfLog.Enable(s => Debug.WriteLine(s));
        }

        /// <summary>
        /// Tests the emitting of messages with all log event levels.
        /// </summary>
        [TestMethod]
        public void EmitMessagesWithAllLogEventLevels()
        {
            this.logger = TestHelper.CreateLogger();

            var counter = 0;

            for (var i = 0; i < 6; i++)
            {
                var template = $"{Guid.NewGuid()} {{counter}}";
                this.logger.Write((LogEventLevel)counter, template, i);
                counter++;
                Thread.Sleep(500);
            }

            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Tests the emitting of messages with the omit properties feature enabled.
        /// </summary>
        [TestMethod]
        public void EmitMessagesWithOmittedProperties()
        {
            this.logger = TestHelper.CreateLogger(true);
            this.logger.Debug("Message text {prop}", 4);
            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Tests the emitting of messages with zero buttons.
        /// </summary>
        [TestMethod]
        public void EmitMessagesWithZeroButtons()
        {
            this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(0));
            this.logger.Debug("Message text {prop}", 1);
            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Tests the emitting of messages with one button.
        /// </summary>
        [TestMethod]
        public void EmitMessagesWithOneButton()
        {
            this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(1));
            this.logger.Debug("Message text {prop}", 2);
            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Tests the emitting of messages with two buttons.
        /// </summary>
        [TestMethod]
        public void EmitMessagesWithTwoButtons()
        {
            this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(2));
            this.logger.Debug("Message text {prop}", 3);
            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Tests the emitting of messages with complex data.
        /// </summary>
        [DeploymentItem("TestException.txt")]
        [TestMethod]
        public void EmitMessagesWithComplexData()
        {
            this.logger = TestHelper.CreateLoggerWithCodeTags();
            var data = File.ReadAllText("TestException.txt");
            this.logger.Debug(data);
            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Tests the emitting of messages with all log event levels.
        /// </summary>
        [TestMethod]
        public void EmitMessagesWithTitleTemplate()
        {
            this.logger = TestHelper.CreateLogger("{Tenant} {Level} {Message}");
            this.logger.Debug("Message text {prop}", 2);
            Thread.Sleep(1000);
            Log.CloseAndFlush();
        }
    }
}
