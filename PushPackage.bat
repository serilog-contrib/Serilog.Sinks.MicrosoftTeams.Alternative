dotnet nuget push "src\Serilog.Sinks.MicrosoftTeams\bin\Release\Serilog.Sinks.MicrosoftTeams.*.nupkg" -s "nuget.org" --skip-duplicate -k "%NUGET_API_KEY%"
PAUSE