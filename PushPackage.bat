dotnet nuget push "src\Serilog.Sinks.MicrosoftTeams.Alternative\bin\Release\Serilog.Sinks.MicrosoftTeams.Alternative.*.nupkg" -s "nuget.org" --skip-duplicate -k "%NUGET_API_KEY%"
PAUSE