namespace Serilog.Sinks.MicrosoftTeams.Alternative.Tests.Extensions;

internal static class WireMockExtensions
{
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