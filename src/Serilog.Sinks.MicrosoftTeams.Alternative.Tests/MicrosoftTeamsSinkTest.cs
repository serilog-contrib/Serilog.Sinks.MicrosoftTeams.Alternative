// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftTeamsSinkTest.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A test class to test the Microsoft Teams sink for basic functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Tests;

/// <summary>
/// A test class to test the Microsoft Teams sink for basic functionality.
/// </summary>
[TestClass]
public class MicrosoftTeamsSinkTest
{
    /// <summary>
    /// The buttons.
    /// </summary>
    private readonly List<MicrosoftTeamsSinkOptionsButton> buttons = new()
    {
        new MicrosoftTeamsSinkOptionsButton { Name = "Google", Uri = "https://google.com" },
        new MicrosoftTeamsSinkOptionsButton { Name = "DuckDuckGo", Uri = "https://duckduckgo.com" }
    };

    /// <summary>
    /// The logger.
    /// </summary>
    private ILogger? logger;

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
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
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

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            6,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages with the omit properties feature enabled.
    /// </summary>
    [TestMethod]
    public void EmitMessagesWithOmittedProperties()
    {
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        this.logger = TestHelper.CreateLogger(true);
        this.logger.Debug("Message text {prop}", 4);
        Thread.Sleep(1000);
        Log.CloseAndFlush();

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages with zero buttons.
    /// </summary>
    [TestMethod]
    public void EmitMessagesWithZeroButtons()
    {
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(0));
        this.logger.Debug("Message text {prop}", 1);
        Thread.Sleep(1000);
        Log.CloseAndFlush();

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages with one button.
    /// </summary>
    [TestMethod]
    public void EmitMessagesWithOneButton()
    {
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(1));
        this.logger.Debug("Message text {prop}", 2);
        Thread.Sleep(1000);
        Log.CloseAndFlush();

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages with two buttons.
    /// </summary>
    [TestMethod]
    public void EmitMessagesWithTwoButtons()
    {
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        this.logger = TestHelper.CreateLoggerWithButtons(this.buttons.Take(2));
        this.logger.Debug("Message text {prop}", 3);
        Thread.Sleep(1000);
        Log.CloseAndFlush();

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages with complex data.
    /// </summary>
    [DeploymentItem("TestException.txt")]
    [TestMethod]
    public void EmitMessagesWithComplexData()
    {
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        this.logger = TestHelper.CreateLoggerWithCodeTags();
        var data = File.ReadAllText("TestException.txt");
        this.logger.Debug(data);
        Thread.Sleep(1000);
        Log.CloseAndFlush();
    }

    /// <summary>
    /// Tests the emitting of messages with a title template (As requested in https://github.com/serilog-contrib/Serilog.Sinks.MicrosoftTeams.Alternative/issues/12).
    /// </summary>
    [TestMethod]
    public void EmitMessagesWithTitleTemplate()
    {
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        this.logger = TestHelper.CreateLogger("My title: {Tenant}");
        this.logger.Debug("Message text {prop} for tenant {Tenant}", 1, "Tenant1");
        Thread.Sleep(1000);
        Log.CloseAndFlush();

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages only with the required filter property.
    /// </summary>
    [TestMethod]
    public void EmitMessagesFilteredByProperty()
    {
        //Arrange
        const string filterOnProperty = "MsTeams";
        
        var logLevels = Enum.GetValues<LogEventLevel>();
        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        int counter = 0;

        this.logger = TestHelper.CreateLoggerWithChannels(new MicrosoftTeamsSinkChannelHandlerOptions(filterOnProperty));

        //Act

        //A message for every loglevel with the required property
        foreach (var logEventLevel in logLevels)
        {
            var loggerWithContext = this.logger.ForContext(filterOnProperty, $"Channel{logEventLevel}");

            var template = $"{Guid.NewGuid()} {{counter}}";
            loggerWithContext.Write(logEventLevel, template, counter);
            counter++;
        }

        //A message for every loglevel without the required property
        foreach (var logEventLevel in logLevels)
        {
            var template = $"{Guid.NewGuid()} {{counter}}";
            this.logger.Write(logEventLevel, template, counter);
            counter++;
        }

        Thread.Sleep(1000);
        Log.CloseAndFlush();

        //Assert

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            logLevels.Length,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages for a specific channel
    /// </summary>
    [TestMethod]
    public void EmitMessagesForSpecificChannel()
    {
        //Arrange
        const string filterOnProperty = "MsTeams";
        const string channelName = "ITTeam";

        using var mockServer = TestHelper.CreateMockServer();
        mockServer.AddChannel(channelName);

        this.logger = TestHelper.CreateLoggerWithChannels(
            new MicrosoftTeamsSinkChannelHandlerOptions(
                filterOnProperty,
                new Dictionary<string, string>
                {
                    [channelName] = $"{mockServer.Url}/{channelName}/"
                }
            )
        );

        //Act

        var loggerWithContext = this.logger.ForContext(filterOnProperty, channelName);
        loggerWithContext.Information("Demo for a specific channel");

        Thread.Sleep(1000);
        Log.CloseAndFlush();

        //Assert
        
        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages for multiple channels
    /// </summary>
    [TestMethod]
    public void EmitMessagesForMultipleChannels()
    {
        //Arrange
        const string filterOnProperty = "MsTeams";
        const string channelName = "ITTeam";
        const string alternativeChannelName = "SupportTeam";
        const string missingChannelName = "HelpdeskTeam";

        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        mockServer.AddChannel(channelName);
        mockServer.AddChannel(alternativeChannelName);

        var channelDictionary = new Dictionary<string, string>
        {
            [channelName] = $"{mockServer.Url}/{channelName}/",
            [alternativeChannelName] = $"{mockServer.Url}/{alternativeChannelName}/"
        };

        this.logger = TestHelper.CreateLoggerWithChannels(
            new MicrosoftTeamsSinkChannelHandlerOptions(
                filterOnProperty,
                channelDictionary
            )
        );

        //Act

        var loggerForDefaultChannel = this.logger.ForContext(filterOnProperty, missingChannelName);
        loggerForDefaultChannel.Information("Demo for the default channel");

        foreach (var channelPair in channelDictionary)
        {
            var loggerForChannel = this.logger.ForContext(filterOnProperty, channelPair.Key);
            loggerForChannel.Information("Demo for the channel {channel}", channelPair.Key);
        }

        Thread.Sleep(1000);
        Log.CloseAndFlush();

        //Assert

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );
        
        foreach (var channelPair in channelDictionary)
        {
            Assert.AreEqual(
                1,
                mockServer.LogEntries.Count(t => t.RequestMessage.Url == channelPair.Value),
                $"Wrong event count for the channel {channelPair.Key}"
            );
        }

        Assert.AreEqual(
            channelDictionary.Count + 1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages for multiple channels using the appsettings.TwoChannelsExample.json configuration
    /// </summary>
    [TestMethod]
    public void EmitMessagesForMultipleChannelsUsingAppSettingsTwoChannelsExample()
    {
        //Arrange
        const string filterOnProperty = "MsTeams";
        const string channelName = "ITTeam";
        const string alternativeChannelName = "SupportTeam";
        const string missingChannelName = "HelpdeskTeam";

        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        mockServer.AddChannel(channelName);
        mockServer.AddChannel(alternativeChannelName);

        var channelDictionary = new Dictionary<string, string>
        {
            [channelName] = $"{mockServer.Url}/{channelName}/",
            [alternativeChannelName] = $"{mockServer.Url}/{alternativeChannelName}/"
        };

        this.logger = TestHelper.CreateLoggerFromConfiguration("appsettings.TwoChannelsExample.json");

        //Act

        var loggerForDefaultChannel = this.logger.ForContext(filterOnProperty, missingChannelName);
        loggerForDefaultChannel.Information("Demo for the default channel");

        foreach (var channelPair in channelDictionary)
        {
            var loggerForChannel = this.logger.ForContext(filterOnProperty, channelPair.Key);
            loggerForChannel.Information("Demo for the channel {channel}", channelPair.Key);
        }

        Thread.Sleep(1000);
        Log.CloseAndFlush();

        //Assert
        
        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        foreach (var channelPair in channelDictionary)
        {
            Assert.AreEqual(
                1,
                mockServer.LogEntries.Count(t => t.RequestMessage.Url == channelPair.Value),
                $"Wrong event count for the channel {channelPair.Key}"
            );
        }

        Assert.AreEqual(
            channelDictionary.Count + 1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }

    /// <summary>
    /// Tests the emitting of messages to the default channel when the specific channel is not found
    /// </summary>
    [TestMethod]
    public void EmitMessagesForDefaultChannel()
    {
        //Arrange
        const string filterOnProperty = "MsTeams";
        const string channelName = "ITTeam";
        const string missingChannelName = "SupportTeam";

        using var mockServer = TestHelper.CreateMockServerWithDefaultChannel();
        
        this.logger = TestHelper.CreateLoggerWithChannels(
            new MicrosoftTeamsSinkChannelHandlerOptions(
                filterOnProperty,
                new Dictionary<string, string>
                {
                    [channelName] = $"{mockServer.Url}/{channelName}/"
                }
            )
        );

        //Act

        var loggerWithContext = this.logger.ForContext(filterOnProperty, missingChannelName);
        loggerWithContext.Information("Demo for a missing channel emiting to default");

        Thread.Sleep(1000);
        Log.CloseAndFlush();

        //Assert

        Assert.IsTrue(
            mockServer
                .LogEntries
                .All(t => t.PartialMatchResult.IsPerfectMatch),
            "Invalid requests made to the mock server"
        );

        Assert.AreEqual(
            1,
            mockServer.LogEntries.Count(),
            "Wrong number of events sent to teams");
    }
}
