dotnet build Serilog.Sinks.MicrosoftTeams.sln -c Release
xcopy /s .\Serilog.Sinks.MicrosoftTeams\bin\Release ..\Nuget\Source\
pause