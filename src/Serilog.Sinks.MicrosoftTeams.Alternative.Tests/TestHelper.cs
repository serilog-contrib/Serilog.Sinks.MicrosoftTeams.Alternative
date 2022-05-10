// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A helper class for the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Tests;

using Extensions;
using Microsoft.Extensions.Configuration;
using WireMock.Server;

/// <summary>
/// A helper class for the tests.
/// </summary>
public static class TestHelper
{
    /// <summary>
    /// The default port for the mock http server
    /// </summary>
    private static readonly int MockServerPort = 63210;

    /// <summary>
    /// The test web hook URL.
    /// </summary>
    private static readonly string TestWebHook = Environment.GetEnvironmentVariable("MicrosoftTeamsWebhookUrl") ?? $"http://localhost:{MockServerPort}";

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

    /// <summary>
    /// Creates the logger with a channel handler.
    /// </summary>
    /// <param name="channelHandler">Channel handler</param>
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
    /// Creates the logger from a appsettings file
    /// </summary>
    /// <param name="appsettingsPath">Path for the appsettings file to use</param>
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
    /// Creates a mock http server with the default channel
    /// </summary>
    /// <returns>A mocked server</returns>
    public static WireMockServer CreateMockServerWithDefaultChannel()
    {
        var server = WireMockServer.Start(MockServerPort);
        server.AddDefaultChannel();

        return server;
    }

    /// <summary>
    /// Creates a clean mock http server
    /// </summary>
    /// <returns>A mocked server</returns>
    public static WireMockServer CreateMockServer()
    {
        var server = WireMockServer.Start(MockServerPort);

        return server;
    }
}
