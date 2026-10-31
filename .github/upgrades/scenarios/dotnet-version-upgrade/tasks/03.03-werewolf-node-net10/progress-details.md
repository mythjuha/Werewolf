# Progress Details: 03.03-werewolf-node-net10

## Changes
- Retargeted Werewolf Node to `net10.0-windows` because it accesses the Windows registry and runs with the Windows-specific application deployment.
- Removed obsolete framework-included System.* PackageReferences and explicit framework assembly references.
- Updated EntityFramework to 6.5.2 and Newtonsoft.Json to 13.0.4.
- Removed unused BotanIO.Net and System.Net.Http.Formatting.Extension package references that were incompatible with net10 and had no source usages.
- Fixed the post-build language XML copy command to use `$(MSBuildProjectDirectory)` and `$(TargetDir)`, allowing direct project builds.
- Replaced RNGCryptoServiceProvider with RandomNumberGenerator.Create, Assembly.CodeBase with AppContext.BaseDirectory, ServicePointManager configuration with no-op removal, and WebClient download with HttpClient.

## Validation
- Direct `dotnet build WerewolfNode.csproj` succeeds and produces `Werewolf Node/bin/Debug/net10.0-windows/Werewolf Node.dll`.
- All project references Database, Shared, and TcpFramework build successfully on net10 during the Node build.
- The build has remaining CA1416 warnings from the large generated `Werewolf.cs` source and legacy Windows-specific application APIs; these are non-blocking analyzer warnings requiring broader generated-code/platform annotation cleanup.

## Deferred Work
- Telegram.Bot remains at the existing 19.0.0-preview.1 API because package/API modernization is complex and outside this mechanical TFM migration.
- Binding redirect/runtime behavior and generated-code analyzer cleanup require runtime validation and are deferred to final validation/follow-up work.
