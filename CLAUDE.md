# Project rules for Claude

## What this is

Serilog.Sinks.MicrosoftTeams.Alternative is a Serilog sink that posts log events to a Microsoft
Teams channel over an incoming webhook or a Power Automate workflow URL. It is published as the
NuGet package `Serilog.Sinks.MicrosoftTeams.Alternative` (`GeneratePackageOnBuild` is on, so every
build produces a `.nupkg` and a `.snupkg`). The project is a maintained fork of
[DixonDs/serilog-sinks-teams](https://github.com/DixonDs/serilog-sinks-teams).

One solution `src/Serilog.Sinks.MicrosoftTeams.Alternative.sln` with exactly two projects:

- `src/Serilog.Sinks.MicrosoftTeams.Alternative/Serilog.Sinks.MicrosoftTeams.Alternative.csproj`,
  the library, multi-targeting `net8.0;net10.0`.
- `src/Serilog.Sinks.MicrosoftTeams.Alternative.Tests/Serilog.Sinks.MicrosoftTeams.Alternative.Tests.csproj`,
  MSTest plus WireMock.Net, single target `net10.0`, `IsPackable=false`.

Layout inside `src/Serilog.Sinks.MicrosoftTeams.Alternative`:

- `LoggerConfigurationMicrosoftTeamsExtensions.cs`: the public entry point, namespace `Serilog`, two
  `WriteTo.MicrosoftTeams(...)` overloads. One takes the individual parameters and builds a
  `MicrosoftTeamsSinkOptions`, the other takes that options object. Both end in the same line that
  wraps `MicrosoftTeamsSink` into a `PeriodicBatchingSink`.
- `Sinks/MicrosoftTeams/Alternative/MicrosoftTeamsSink.cs`: the sink itself, an
  `IBatchedLogEventSink`. `EmitBatchAsync` groups the events with `GetMessagesToSend` and posts them
  with `PostMessages`. The private helpers `CreateMessageCard`, `CreateMessage`, `GetChannelUri`,
  `GetRenderedMessage`, `GetRenderedTitle` and `GetFacts` do one thing each, keep new logic in that
  shape.
- `Sinks/MicrosoftTeams/Alternative/MicrosoftTeamsSinkOptions.cs`: every configuration value, all
  get-only, validated in the constructor.
  `MicrosoftTeamsSinkChannelHandlerOptions.cs` holds the multi channel configuration,
  `MicrosoftTeamsSinkOptionsButton.cs` a single URL button,
  `MicrosoftExtendedLogEvent.cs` a log event plus its first and last occurrence.
- `Sinks/MicrosoftTeams/Alternative/Core/`: the JSON payload types. `MicrosoftTeamsMessageCard` and
  its `Section`/`Fact`/`Action` types are the legacy MessageCard format, `MicrosoftTeamsMessage` and
  `MicrosoftTeamsAttachment` are the adaptive card format for Power Automate workflows.
- `Sinks/MicrosoftTeams/Alternative/Constants/`, `Enumerations/`, `Exceptions/`, `Extensions/`: one
  type each (`SinkConstants`, `MicrosoftTeamsColors`, `LoggingException`, `EnumerableExtensions`).
- `GlobalUsings.cs`: all usings of the project.

Layout inside `src/Serilog.Sinks.MicrosoftTeams.Alternative.Tests`:

- `MicrosoftTeamsSinkTest.cs`: the only test class. It logs through a real `LoggerConfiguration`
  against a WireMock server and asserts on the requests that arrive there.
- `TestHelper.cs`: the `CreateLogger*` factories and the two mock server factories.
- `Extensions/WireMockExtensions.cs`: `AddDefaultChannel` and `AddChannel`, the request matchers
  the assertions rely on.
- `appsettings.TwoChannelsExample.json` and `TestException.txt`: copied to the output directory with
  `CopyToOutputDirectory=Always`.
- `GlobalUsings.cs`: all usings of the test project.

Repository root: `README.md` (badges, target frameworks, contributor table), `HowToUse.md` (the
actual user documentation with the option table), `Changelog.md`, `Updating.md` (the five step
release checklist), `License.txt` (MIT), `Icon.png` (the package icon),
`BuildAndPushPackage.bat`, `Delete-BIN-OBJ-Folders.bat` and `.all-contributorsrc`. There is no
`.github` folder and no pipeline file, the repository has no CI.

`src/Directory.Build.props` exists but sets exactly one property, `GenerateDocumentationFile`.
Everything else lives directly in the two `.csproj` files and is duplicated there.

## Build

```powershell
dotnet build src/Serilog.Sinks.MicrosoftTeams.Alternative.sln
```

```powershell
dotnet test src/Serilog.Sinks.MicrosoftTeams.Alternative.sln
```

- **Restore needs nuget.org and this machine has a private feed configured globally.** That feed
  answers with a connection refused, which fails restore with `NU1301` and, because `NuGetAudit` is
  on, additionally with `NU1900`. Build and test with an explicit source then:
  `dotnet build src/Serilog.Sinks.MicrosoftTeams.Alternative.sln --source https://api.nuget.org/v3/index.json`.
  `dotnet list package --outdated` does not forward `--source` to its implicit restore, use
  `--no-restore` there after a successful build.
- `TreatWarningsAsErrors` is enabled in both projects, so every warning breaks the build, NuGet
  warnings (`NU****`) from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- `GenerateDocumentationFile` from `src/Directory.Build.props` plus `TreatWarningsAsErrors` means a
  missing XML doc comment (`CS1591`) is a build error, not a hint.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.5.1-1` for the first
  commit after tag `1.5.0`. Never edit a version property or an assembly version by hand.
- The library multi-targets, so `dotnet build` produces `bin/Release/net8.0` and
  `bin/Release/net10.0` plus the package next to them in `bin/Release`.
- Tests are MSTest with WireMock.Net, in the single test project. `dotnet test` runs 13 tests in
  about three seconds. They need no internet, but they do need the local TCP port **63210**, and
  they only pass when the two webhook environment variables are unset, see "Known quirks". Never
  claim a test run happened without running it.
- A behaviour change that the WireMock tests cannot see (the actual card rendering in Teams) is
  verified by setting `MicrosoftTeamsWebhookUrl` or `MicrosoftTeamsWebhookUrlPowerAutomate` and
  looking at the channel. Read the warning about those variables under "Known quirks" first.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with
  `<copyright file="..." company="SeppPenner and the Serilog contributors">` and a `<summary>`, then
  the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
  Implementations of an interface member additionally carry `<inheritdoc cref="..."/>` pointing at
  that interface.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into the `GlobalUsings.cs` of the respective project, inside the
  existing `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig
  requires usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`),
  which global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas
  except the `Serilog004` ones the tests already use. The comment text in that block is German
  because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.
- The `.csproj` files are indented with four spaces, unlike the Visual Studio default of two.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The webhook environment variables hijack the tests.** `TestHelper` reads
  `MicrosoftTeamsWebhookUrl` and `MicrosoftTeamsWebhookUrlPowerAutomate` and only falls back to
  `http://localhost:63210` when they are empty. If either variable is set on the machine, the test
  run posts real messages into a real Teams channel and every assertion against the WireMock server
  fails, because nothing ever reaches it. Check the variables before debugging a red test run.
- **The mock server port is fixed.** `TestHelper.MockServerPort` is `63210` and every test starts
  its own `WireMockServer` on it. Two test runs at the same time, or any other process holding that
  port, break the run. This is also why the tests must not be parallelized.
- **`RootNamespace` is `Serilog`, not the assembly name.** The extension class therefore lives in
  namespace `Serilog` at the project root, so that `WriteTo.MicrosoftTeams(...)` is available
  without an extra using. Everything else sits in the folder chain
  `Sinks/MicrosoftTeams/Alternative/...`, which mirrors the namespace
  `Serilog.Sinks.MicrosoftTeams.Alternative`.
- **Two payload formats in one sink.** `UsePowerAutomateWorkflows` decides between
  `CreateMessageCard` (the legacy `MessageCard` JSON for O365 incoming webhooks) and `CreateMessage`
  (an `AdaptiveCards.AdaptiveCard` inside an attachment, for Power Automate workflow URLs). Both
  paths build title, text, facts and buttons separately, a change to one of them usually has to be
  made in the other as well.
- **AdaptiveCards types are always written with their namespace prefix**
  (`AdaptiveCards.AdaptiveTextBlock`), there is deliberately no global using for that namespace, so
  that the adaptive card code is recognizable at a glance.
- **Events are grouped by exception message only.** `GetMessagesToSend` merges a log event into an
  earlier one of the same batch when both exception messages are equal, no matter how different the
  rendered messages are. The merged event keeps the first one's text and only widens the first and
  last occurrence. With the default `BatchSizeLimit` of 1 this never happens.
- **`System.Net.Http` is deliberately not referenced any more.** The package was added in version
  1.4.1.0/1.4.2.0 against the `System.Runtime` loading issue
  [#30](https://github.com/serilog-contrib/Serilog.Sinks.MicrosoftTeams.Alternative/issues/30). On
  net8.0 and later the framework provides those assemblies, the reference did nothing, and the Net
  10 SDK rejects it with `NU1510`. Do not add it back, add a `NoWarn` for it even less.
- **The tests close their logger, they do not sleep.** Every test ends with `CloseLogger`, which
  disposes the logger and thereby flushes the batching sink. `Thread.Sleep` plus
  `Log.CloseAndFlush()` used to stand there, which was wrong twice over: `Log.Logger` is never
  assigned by these tests, and one second is not enough to drain a queue that the sink empties one
  event per second. Leftover events then arrived at the mock server of the following test and made
  its count assertion fail.
- **The `Serilog004` pragmas in the tests are required.** Several tests build their message template
  from a `Guid` at runtime, which the Serilog analyzer rejects. The pragma pairs sit around exactly
  those calls, do not widen them to whole files.
- **`failureCallback` is gone but still documented.** The option was removed in commit `2f3e3c0`,
  the row in `HowToUse.md` remains as struck through text so that readers of older versions find
  the replacement (fallback logging, see https://nblumhardt.com/2024/10/fallback-logging/).
- **`Updating.md`, `HowToUse.md` and `README.md` each own a part of the documentation.** The option
  table lives in `HowToUse.md` only, the README links to it. A new sink option has to be added to
  the option table, both `MicrosoftTeams(...)` overloads and `MicrosoftTeamsSinkOptions`.
- **The contributor table is generated.** The block between the `ALL-CONTRIBUTORS-LIST` markers in
  `README.md` and the file `.all-contributorsrc` belong to the all-contributors bot. Do not edit
  them by hand.
- **`origin` points at the old repository name.** The remote is
  `https://github.com/SeppPenner/Serilog.Sinks.MicrosoftTeams`, every URL in the code and the
  documentation points at `serilog-contrib/Serilog.Sinks.MicrosoftTeams.Alternative`. GitHub
  redirects the old path, so fetch and push work, but do not "fix" URLs in files to match the
  remote, it is the remote that is out of date.
- **The two batch files must run from the repository root.** Both start with `cd .\src`, that path
  is relative to the working directory, not to the script.
- **The changelog version has four parts, the tag has three.** `1.5.0.0` in `Changelog.md` and in
  `PackageReleaseNotes`, `1.5.0` as the tag.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.6.0.0 (2026-08-17)**: Short description.`
3. Put the same text into `PackageReleaseNotes` in
   `src/Serilog.Sinks.MicrosoftTeams.Alternative/Serilog.Sinks.MicrosoftTeams.Alternative.csproj`,
   in the format `Version 1.6.0.0 (2026-08-17): Short description.`.
4. Commit that.
5. Tag the commit with the plain version number, no `v` prefix (`1.5.0`, `1.4.9`, ...). The existing
   tags are lightweight tags, create new ones the same way.
6. Push the commits and the tag.
7. Only now run `BuildAndPushPackage.bat` from the repository root. It deletes every `bin` and `obj`
   below `src`, builds in Release and pushes `*.nupkg` and `*.snupkg` to nuget.org with
   `%NUGET_API_KEY%`.

The tag has to exist **before** the package build, otherwise GitVersion burns a prerelease version
such as `1.6.0-1+Branch.master.Sha...` into the published package. `Updating.md` describes the same
order in shorter words.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
