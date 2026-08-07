# PhotinoX.App.Dashboard

Demonstrates `PhotinoX.App` application composition with dependency injection, configuration, environment, initialization services, WebView messaging, runtime diagnostics, `NewWindowRequested`, and named window configuration.

The sample supports Native AOT publishing. `PhotinoX.App` uses AOT-friendly configuration binding so strongly typed application settings can be used in trim/AOT scenarios.

## Projects

- `PhotinoX.App.Dashboard.csproj` shows explicit event-based window setup.
- `PhotinoX.App.Dashboard.Fluent.csproj` shows the same dashboard using fluent window handler registration.

## Run explicit sample

```bash
dotnet run --project PhotinoX.App.Dashboard.csproj
```

## Run fluent sample

```bash
dotnet run --project PhotinoX.App.Dashboard.Fluent.csproj
```

## Publish Native AOT

Publish explicit sample:

```bash
dotnet publish PhotinoX.App.Dashboard.csproj -c Release -f net10.0 -r win-x64 --self-contained true
```

Publish fluent sample:

```bash
dotnet publish PhotinoX.App.Dashboard.Fluent.csproj -c Release -f net10.0 -r win-x64 --self-contained true
```