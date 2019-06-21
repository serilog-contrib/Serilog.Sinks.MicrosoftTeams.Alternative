dotnet build Serilog.Sinks.MicrosoftTeams.sln -c Release
xcopy /s .\Serilog.Sinks.MicrosoftTeams\bin\Release ..\Nuget\Source\
xcopy /s .\Help ..\Nuget\Documentation\
pause