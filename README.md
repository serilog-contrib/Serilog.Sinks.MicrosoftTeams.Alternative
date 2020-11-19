Serilog.Sinks.MicrosoftTeams
====================================

Serilog.Sinks.MicrosoftTeams is a library to save logging information from [Serilog](https://github.com/serilog/serilog) to [Microsoft Teams](https://products.office.com/en-us/microsoft-teams/group-chat-software).
The assembly was written and tested in Net 5.0

[![Build status](https://ci.appveyor.com/api/projects/status/x4l2tdvyj7gv51qo?svg=true)](https://ci.appveyor.com/project/SeppPenner/serilog-sinks-microsoftteams)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/Serilog.Sinks.MicrosoftTeams.svg)](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/Serilog.Sinks.MicrosoftTeams.svg)](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/Serilog.Sinks.MicrosoftTeams.svg)](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/master/License.txt)
[![Nuget](https://img.shields.io/badge/Serilog.Sinks.MicrosoftTeams-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.Serilog.Sinks.MicrosoftTeams/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/Serilog.Sinks.MicrosoftTeams/badge.svg)](https://snyk.io/test/github/SeppPenner/Serilog.Sinks.MicrosoftTeams)
[![Gitter](https://badges.gitter.im/Serilog-Sinks-MicrosoftTeams/community.svg)](https://gitter.im/Serilog-Sinks-MicrosoftTeams/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## Available for
* NetFramework 4.6
* NetFramework 4.6.2
* NetFramework 4.7
* NetFramework 4.7.2
* NetFramework 4.8
* NetStandard 2.0
* NetStandard 2.1
* NetCore 2.1
* NetCore 3.1
* Net 5.0

## Net Core and Net Framework latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet-framework
* https://dotnet.microsoft.com/download/dotnet-core
* https://dotnet.microsoft.com/download/dotnet/5.0

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
|proxy|The proxy addresss used.|`proxy: "http://test.de/proxy"`|`null`|
|buttons|Add static clickable buttons to each message.|`buttons: new[] { new MicrosoftTeamsSinkOptionsButton("Google", "https://google.se") }`|`null`|

## Further information:
This project is a fork of https://github.com/DixonDs/serilog-sinks-teams but is maintained.
Do not hesitate to create [issues](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/issues) or [pull requests](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/pulls).

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams/blob/master/Changelog.md).
