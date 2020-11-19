dotnet nuget push "src\Serilog.Sinks.MicrosoftTeams\bin\Release\HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams.*.nupkg" -s "github" --skip-duplicate
dotnet nuget push "src\Serilog.Sinks.MicrosoftTeams\bin\Release\HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams.*.nupkg" -s "nuget.org" --skip-duplicate -k "%NUGET_API_KEY%"
PAUSE