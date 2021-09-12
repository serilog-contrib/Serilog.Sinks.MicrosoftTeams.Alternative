## Basic usage
You need to add an "Incoming Webhook" connector to your Teams channel and get it's URL. `titleTemplate` is optional but can help your distinguish logs coming from different sources.
Check https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/connectors/connectors-using.

```csharp
var logger = new LoggerConfiguration()
	.WriteTo.MicrosoftTeams(webHookUri, titleTemplate: titleTemplate)
    .CreateLogger();
```

The project can be found on [nuget](https://www.nuget.org/packages/Serilog.Sinks.MicrosoftTeams.Alternative/).

## Configuration options
|Parameter|Meaning|Example|Default value|
|-|-|-|-|
|webHookUri|The Microsoft teams weebhook uri.|`https://outlook.office.com/webhook/1234567890`|None, is mandatory.|
|titleTemplate|The title template of the card. The Serilog templating can be used to customize the title (Same as the `outputTemplate` property).|`"Some Message"`|None, but is optional.|
|batchSizeLimit|The maximum number of events to include in a single batch.|`batchSizeLimit: 40`|`1`|
|period|The time to wait between checking for event batches.|`period: new TimeSpan(0, 0, 20)`|`00:00:01`|
|outputTemplate|The output template for a log event.|`outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"`|`null`|
|formatProvider|The `IFormatProvider` to use. Supplies culture-specific formatting information. Check https://docs.microsoft.com/en-us/dotnet/api/system.iformatprovider?view=net-5.0.|`new CultureInfo("de-DE")`|`null`|
|restrictedToMinimumLevel|The minimum level of the logging.|`restrictedToMinimumLevel: LogEventLevel.Verbose`|`LogEventLevel.Verbose`|
|omitPropertiesSection|Indicates whether the properties section should be omitted or not.|`omitPropertiesSection: true`|`false`|
|useCodeTagsForMessage|A value indicating whether code tags are used for the message template or not. This is useful if you have complex messages that might get formatted as unwanted markdown elements.|`useCodeTagsForMessage:true`|`false`|
|proxy|The proxy addresss used.|`proxy: "http://test.de/proxy"`|`null`|
|buttons|Option to add static clickable buttons to each message.|`buttons: new[] { new MicrosoftTeamsSinkOptionsButton("Google", "https://google.de") }`|`null`|
|failureCallback|Adds an option to add a failure callback action.|`failureCallback: e => Console.WriteLine($"Sink error: {e.Message}")`|`null`|
|queueLimit|The maximum number of events that should be stored in the batching queue.|`queueLimit: 10`|`int.MaxValue` or `2147483647`|

## Further information
This project is a fork of https://github.com/DixonDs/serilog-sinks-teams but is maintained.
Do not hesitate to create [issues](https://github.com/serilog-contrib/Serilog.Sinks.MicrosoftTeams.Alternative/issues) or [pull requests](https://github.com/serilog-contrib/Serilog.Sinks.MicrosoftTeams.Alternative/pulls).
The relevant reference from Microsoft can be found here: https://docs.microsoft.com/en-us/outlook/actionable-messages/message-card-reference.