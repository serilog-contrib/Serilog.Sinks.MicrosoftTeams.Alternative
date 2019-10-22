Serilog.Sinks.MicrosoftTeams
====================================

Serilog.Sinks.MicrosoftTeams is a library to save logging information from [Serilog](https://github.com/serilog/serilog) to [Microsoft Teams](https://products.office.com/en-us/microsoft-teams/group-chat-software).
The assembly was written and tested in .Net Framework 4.8 and .Net Standard 2.0.

[![Build status](https://ci.appveyor.com/api/projects/status/x4l2tdvyj7gv51qo?svg=true)](https://ci.appveyor.com/project/SeppPenner/serilog-sinks-microsoftteams)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/Serilog.Sinks.MicrosoftTeams.svg)](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/Serilog.Sinks.MicrosoftTeams.svg)](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/Serilog.Sinks.MicrosoftTeams.svg)](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/stargazers)
[![GitHub license](https://img.shields.io/badge/license-AGPL-blue.svg)](https://raw.githubusercontent.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/master/License.txt)
[![Nuget](https://img.shields.io/badge/Serilog.Sinks.MicrosoftTeams-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/Serilog.Sinks.MicrosoftTeams/badge.svg)](https://snyk.io/test/github/SeppPenner/Serilog.Sinks.MicrosoftTeams)

## Available for
* NetFramework 4.5
* NetFramework 4.6
* NetFramework 4.6.2
* NetFramework 4.7
* NetFramework 4.7.2
* NetFramework 4.8
* NetStandard 2.0
* NetCore 2.2
* NetCore 3.0

## Basic usage:
You need to add an "Incoming Webhook" connector to your Teams channel and get it's URL. `title` is optional but can help your distinguish logs coming from different sources.
Check https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/connectors/connectors-using.

```csharp
var logger = new LoggerConfiguration()
	.WriteTo.MicrosoftTeams(webHookUri, title: title)
    .CreateLogger();
```

The project can be found on [nuget](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams/).

## Configuration options:

|Parameter|Meaning|Example|Default value|
|-|-|-|-|
|webHookUri|The Microsoft teams weebhook uri.|`"User ID=serilog;Password=serilog;Host=localhost;Port=5432;Database=Logs"`|None, is mandatory.|
|title|The title of the card.|`"Some Message"`|None, but is optional.|
|period|The time to wait between checking for event batches.|`period: new TimeSpan(0, 0, 20)`|`00:00:05`|
|formatProvider|The `IFormatProvider` to use. Supplies culture-specific formatting information. Check https://docs.microsoft.com/en-us/dotnet/api/system.iformatprovider?view=netframework-4.8.|`new CultureInfo("de-DE")`|`null`|
|batchSizeLimit|The maximum number of events to include in a single batch.|`batchSizeLimit: 40`|`30`|
|restrictedToMinimumLevel|The minimum level of the logging.|`restrictedToMinimumLevel: LogEventLevel.Verbose`|`LogEventLevel.Verbose`|
|omitPropertiesSection|Indicates whether the properties section should be omitted or not.|`omitPropertiesSection: true`|`false`|

## Further information:
This project is a fork of https://github.com/DixonDs/serilog-sinks-teams but is maintained.
Do not hesitate to create [issues](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/issues) or [pull requests](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/pulls).

Change history
--------------

* **Version 1.0.3.1 (2019-10-22)** : Removed invalid fields from nuspec file, added dependency information to nuget package, added build for netcore.
* **Version 1.0.3.0 (2019-10-15)** : Added option for omitting properties section in message, added GitVersionTask, updated nuget packages.
* **Version 1.0.2.1 (2019-06-24)** : Added option to only show from and to dates when the dates are not equal.
* **Version 1.0.2.0 (2019-06-23)** : Fixed icon in nuget package.
* **Version 1.0.0.1 (2019-06-21)** : Added option for minimal log level.
* **Version 1.0.0.0 (2019-06-21)** : 1.0 release.