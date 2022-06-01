// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WireMockExtensions.cs" company="SeppPenner and the Serilog contributors">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   An extension class for WireMock tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Serilog.Sinks.MicrosoftTeams.Alternative.Tests.Extensions;

/// <summary>
/// An extension class for WireMock tests.
/// </summary>
internal static class WireMockExtensions
{
    /// <summary>
    /// Adds a default channel to the WireMock server.
    /// </summary>
    /// <param name="server">The WireMock server.</param>
    public static void AddDefaultChannel(this WireMockServer server)
    {
        server.Given(
                Request
                    .Create()
                    .WithPath("/")
                    .WithHeader("content-type", "application/json; charset=utf-8")
                    .UsingPost()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(200)
            );
    }

    /// <summary>
    /// Adds a channel to the WireMock server.
    /// </summary>
    /// <param name="server">The WireMock server.</param>
    /// <param name="channel">The channel.</param>
    public static void AddChannel(this WireMockServer server, string channel)
    {
        server.Given(
                Request
                    .Create()
                    .WithPath($"/{channel}/")
                    .WithHeader("content-type", "application/json; charset=utf-8")
                    .UsingPost()
            )
            .RespondWith(
                Response
                    .Create()
                    .WithStatusCode(200)
            );
    }
}