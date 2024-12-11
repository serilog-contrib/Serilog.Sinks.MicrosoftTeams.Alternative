// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A helper class for the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Tests;

/// <summary>
/// A helper class for the tests.
/// </summary>
public static class TestHelper
{
    /// <summary>
    /// The default port for the mock http server.
    /// </summary>
    private static readonly int MockServerPort = 63210;

    /// <summary>
    /// The test web hook URL.
    /// </summary>
    private static readonly string TestWebHook = Environment.GetEnvironmentVariable("MicrosoftTeamsWebhookUrl") ?? $"http://localhost:{MockServerPort}";

    /// <summary>
    /// The test web hook URL for Power Automate.
    /// </summary>
    private static readonly string TestWebHookPowerAutomate = Environment.GetEnvironmentVariable("MicrosoftTeamsWebhookUrlPowerAutomate") ?? $"http://localhost:{MockServerPort}";

    /// <summary>
    /// Creates the logger.
    /// </summary>
    /// <param name="titleTemplate">The title template.</param>
    /// <returns>An <see cref="ILogger"/>.</returns>
    public static ILogger CreateLogger(string titleTemplate)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, titleTemplate))
            .CreateLogger();

        return logger;
    }

    /// <summary>
    /// Creates the logger.
    /// </summary>
    /// <param name="omitPropertiesSection">A value indicating whether the properties should be omitted or not.</param>
    /// <param name="usePowerAutomateWorkflows">A value indicating whether Power Automate workflows are used or not.</param>
    /// <returns>An <see cref="ILogger"/>.</returns>
    public static ILogger CreateLogger(bool omitPropertiesSection = false, bool usePowerAutomateWorkflows = false)
    {
        if (usePowerAutomateWorkflows)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHookPowerAutomate, "Integration Tests", omitPropertiesSection: omitPropertiesSection, usePowerAutomateWorkflows: usePowerAutomateWorkflows))
                .CreateLogger();

            return logger;
        }

        var logger2 = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, "Integration Tests", omitPropertiesSection: omitPropertiesSection, usePowerAutomateWorkflows: usePowerAutomateWorkflows))
            .CreateLogger();

        return logger2;
    }

    /// <summary>
    /// Creates the logger with an URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="omitPropertiesSection">A value indicating whether the properties should be omitted or not.</param>
    /// <param name="usePowerAutomateWorkflows">A value indicating whether Power Automate workflows are used or not.</param>
    /// <returns>An <see cref="ILogger"/>.</returns>
    public static ILogger CreateLoggerWithUrl(string url, bool omitPropertiesSection = false, bool usePowerAutomateWorkflows = false)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(url, "Integration Tests", omitPropertiesSection: omitPropertiesSection, usePowerAutomateWorkflows: usePowerAutomateWorkflows))
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

    /// <summary>
    /// Creates the logger with a channel handler.
    /// </summary>
    /// <param name="channelHandler">The channel handler.</param>
    /// <returns>An <see cref="ILogger"/>.</returns>
    public static ILogger CreateLoggerWithChannels(MicrosoftTeamsSinkChannelHandlerOptions channelHandler)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.MicrosoftTeams(new MicrosoftTeamsSinkOptions(TestWebHook, channelHandler: channelHandler))
            .CreateLogger();

        return logger;
    }

    /// <summary>
    /// Creates the logger from a appsettings file.
    /// </summary>
    /// <param name="appsettingsPath">The path for the appsettings file to use.</param>
    /// <returns>An <see cref="ILogger"/>.</returns>
    public static ILogger CreateLoggerFromConfiguration(string appsettingsPath)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appsettingsPath)
            .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return logger;
    }

    /// <summary>
    /// Creates a mock HTTP server with the default channel.
    /// </summary>
    /// <returns>A mocked HTTP server.</returns>
    public static WireMockServer CreateMockServerWithDefaultChannel()
    {
        var server = WireMockServer.Start(MockServerPort);
        server.AddDefaultChannel();
        return server;
    }

    /// <summary>
    /// Creates a clean mock HTTP server.
    /// </summary>
    /// <returns>A mocked HTTP server.</returns>
    public static WireMockServer CreateMockServer()
    {
        var server = WireMockServer.Start(MockServerPort);
        return server;
    }
}
