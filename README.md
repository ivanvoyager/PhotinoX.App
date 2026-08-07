[![PhotinoX Logo](https://raw.githubusercontent.com/ivanvoyager/PhotinoX/refs/heads/master/assets/photinox-logo.png)](https://github.com/ivanvoyager/PhotinoX)

# PhotinoX.App

[![NuGet Version](https://img.shields.io/nuget/v/PhotinoX.App.svg)](https://www.nuget.org/packages/PhotinoX.App)
[![Build](https://github.com/ivanvoyager/PhotinoX.App/actions/workflows/build.yml/badge.svg)](https://github.com/ivanvoyager/PhotinoX.App/actions/workflows/build.yml)
[![License](https://img.shields.io/github/license/ivanvoyager/PhotinoX.App?label=license)](https://github.com/ivanvoyager/PhotinoX.App/blob/master/LICENSE)
[![NuGet Downloads](https://img.shields.io/nuget/dt/PhotinoX.App.svg)](https://www.nuget.org/packages/PhotinoX.App)

Application-level builder, dependency injection, configuration, logging, environment, and window settings APIs for [PhotinoX](https://github.com/ivanvoyager/PhotinoX) desktop applications.

`PhotinoX` provides the low-level native-first application, dispatcher, and window API. `PhotinoX.App` adds the application composition layer around it: services, configuration, logging, environment paths, initialization services, and reusable window settings.

- service registration through `IServiceCollection`
- configuration through `ConfigurationManager`
- logging through `ILoggingBuilder`
- application environment through `PhotinoEnvironment`
- main-window factory support
- application initialization services
- bindable `PhotinoX` settings from configuration
- default and per-window configuration
- Native AOT friendly configuration binding

## Quick start

Configuration can be provided from `appsettings.json`, environment variables, command-line arguments, or directly through the builder:

```csharp
var builder = PhotinoApp.CreateBuilder(args);

builder.Configuration["PhotinoX:WebRootPath"] = "wwwroot";
builder.Configuration["PhotinoX:MainWindow:Window:Title"] = "PhotinoX.App";
builder.Configuration["PhotinoX:MainWindow:Window:Width"] = "900";
builder.Configuration["PhotinoX:MainWindow:Window:Height"] = "600";
builder.Configuration["PhotinoX:MainWindow:Window:StartUrl"] = "index.html";

builder.UseMainWindow(app =>
{
    return new PhotinoWindow()
        .ApplySettings(app.GetMainWindowConfiguration(), app.Environment);
});

return builder.Build().Run();
```

## Application builder

`PhotinoAppBuilder` is the main composition object.

It exposes:

```csharp
builder.Services
builder.Configuration
builder.Environment
builder.Logging
```

The builder can configure services, application-level callbacks, the main-window factory, application initialization behavior, and custom service-provider creation.

Example using the `appsettings.json` configuration shown below:

```csharp
var builder = PhotinoApp.CreateBuilder(args);

builder.ConfigureApplication(application =>
{
    application.ShutdownMode = PhotinoShutdownMode.OnMainWindowClose;
});

builder.UseMainWindow(app =>
{
    return new PhotinoWindow()
        .ApplySettings(app.GetMainWindowConfiguration(), app.Environment);
});

return builder.Build().Run();
```

## Configuration

`PhotinoApp.CreateBuilder(args)` creates a builder with common defaults:

- `appsettings.json`
- `appsettings.{EnvironmentName}.json`
- environment variables
- command-line arguments
- `PhotinoAppSettings` binding from the `PhotinoX` section
- console logging
- `IConfiguration` registration
- `PhotinoEnvironment` registration

The default configuration section is:

```text
PhotinoX
```

### appsettings.json

```json
{
  "PhotinoX": {
    "ApplicationName": "PhotinoX App",
    "WebRootPath": "wwwroot",

    "WindowDefaults": {
      "Window": {
        "Width": 900,
        "Height": 600,
        "CenterOnInitialize": true,
        "Resizable": true
      },
      "Browser": {
        "DevToolsEnabled": true,
        "ContextMenuEnabled": true
      }
    },

    "MainWindow": {
      "Window": {
        "Title": "PhotinoX App",
        "StartUrl": "index.html"
      }
    },

    "Windows": {
      "Settings": {
        "Window": {
          "Title": "Settings",
          "Width": 700,
          "Height": 500,
          "StartUrl": "settings.html"
        }
      }
    },

    "Runtime": {
      "WebView2RuntimePath": null
    }
  }
}
```

The `PhotinoX` configuration section is bound to `PhotinoAppSettings` and uses this shape:

```text
PhotinoX
  Runtime
  WindowDefaults
  MainWindow
  Windows[name]
```

### Window configuration

Window configuration uses a default plus override model.

For the main window:

```text
WindowDefaults + MainWindow
```

For a named window:

```text
WindowDefaults + Windows[name]
```

Get the effective main window configuration:

```csharp
var configuration = app.GetMainWindowConfiguration();
```

Get a named window configuration:

```csharp
var configuration = app.GetWindowConfiguration("Settings");
```

Apply a full window configuration:

```csharp
var window = new PhotinoWindow().ApplySettings(configuration, app.Environment);
```

## Environment

`PhotinoEnvironment` exposes `EnvironmentName`, `ApplicationName`, `ContentRootPath`, and `WebRootPath`.

Relative startup URLs can be resolved against `WebRootPath`:

```csharp
var resolved = app.Environment.ResolveStartUrl("index.html");
```

## Runtime settings

`PhotinoRuntimeSettings` contains runtime-level settings that are not per-window.

```json
{
  "PhotinoX": {
    "Runtime": {
      "WebView2RuntimePath": "runtimes/webview2"
    }
  }
}
```

`WebView2RuntimePath` is a Windows-only application-level setting for WebView2 fixed-version deployment. It is applied before application configuration callbacks and before windows are created.

## Application initialization services

`IPhotinoInitializeService` can be used for services that need access to the built root service provider before the application starts running.

```csharp
public sealed class MyInitializer : IPhotinoInitializeService
{
    public void Initialize(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<MyInitializer>>();
        logger.LogInformation("Application initialized.");
    }
}
```

Register it:

```csharp
builder.ConfigureServices(services =>
{
    services.AddSingleton<IPhotinoInitializeService, MyInitializer>();
});
```

By default, initialization services run during `PhotinoAppBuilder.Build()`.

Automatic initialization can be disabled:

```csharp
var builder = PhotinoApp.CreateBuilder(new PhotinoAppOptions
{
    Args = args,
    InitializeAppServices = false
});

// Equivalent:
// var builder = PhotinoApp.CreateBuilder(args)
//     .UseAppServicesInitialization(false);

var app = builder.Build();

app.InitializeAppServices();

return app.Run();
```

## Services and logging

`PhotinoX.App` uses `Microsoft.Extensions.DependencyInjection`.

```csharp
builder.ConfigureServices(services =>
{
    services.AddSingleton<MyService>();
    services.AddSingleton<IPhotinoInitializeService, MyInitializer>();
});
```

The built app exposes the root service provider:

```csharp
var app = builder.Build();

var service = app.Services.GetRequiredService<MyService>();
```

Default builder configuration enables console logging and reads settings from the `Logging` configuration section.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

Additional logging configuration can be applied through the builder:

```csharp
builder.Logging.AddFilter("MyApp", LogLevel.Debug);
```

Custom service-provider creation can be configured with `ConfigureContainer(...)`.

```csharp
builder.ConfigureContainer(factory, container =>
{
    // Configure container-specific builder.
});
```

## Ecosystem

`PhotinoX.App` does not replace the `PhotinoX` API. `PhotinoApplication` owns the native desktop lifetime, dispatcher, windows, and message loop. `PhotinoX.App` adds a lightweight application composition layer around it.

Use `PhotinoX` directly for minimal or fully manual applications. Use `PhotinoX.App` when the app needs a modern .NET-style startup model on top of `PhotinoX`.

- [**PhotinoX**](https://github.com/ivanvoyager/PhotinoX) - managed .NET wrapper around the native layer.
- [**PhotinoX.Native**](https://github.com/ivanvoyager/PhotinoX.Native) - native binaries for Windows/macOS/Linux.
- [**PhotinoX.Blazor**](https://github.com/ivanvoyager/PhotinoX.Blazor) - Blazor integration for native desktop apps.
- [**PhotinoX.Server**](https://github.com/ivanvoyager/PhotinoX.Server) - optional local static-file server for SPA/static assets.
- [**PhotinoX.Samples**](https://github.com/ivanvoyager/PhotinoX.Samples) - sample projects showcasing common scenarios.

---

## Install

```bash
dotnet add package PhotinoX.App
```

`PhotinoX.App` depends on `PhotinoX`, which provides the managed API over the native WebView host.
> Package targets **net8.0; net9.0; net10.0**.

## Samples

- [Samples](https://github.com/ivanvoyager/PhotinoX.App/tree/master/Samples)

## Requirements

- **.NET 10 SDK** (build)
- **Target frameworks:** `net8.0; net9.0; net10.0` (package supports all three)
- Runtime deps: see [**PhotinoX.Native**](https://www.nuget.org/packages/PhotinoX.Native) (`runtimes/<rid>/native/`)
- **Windows:** Microsoft Edge WebView2 Runtime  
  https://learn.microsoft.com/microsoft-edge/webview2/
- **macOS:** WKWebView (system WebKit)  
  https://developer.apple.com/documentation/webkit/wkwebview/
- **Linux:** WebKitGTK 4.1 runtime packages  
  https://webkitgtk.org/

## Build from source

```bash
dotnet restore src/PhotinoX.App/PhotinoX.App.csproj
dotnet build   src/PhotinoX.App/PhotinoX.App.csproj -c Release
dotnet pack    src/PhotinoX.App/PhotinoX.App.csproj -c Release -o artifacts
```
> CI: see [`.github/workflows/build.yml`](https://github.com/ivanvoyager/PhotinoX.App/blob/master/.github/workflows/build.yml) (build + pack + upload `.nupkg`/`.snupkg`).

## Contributing

Issues and PRs are welcome. Keep PRs focused, minimal, and consistent with the rest of PhotinoX.

## License

PhotinoX.App is licensed under **Apache‑2.0**.