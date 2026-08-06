using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Photino.NET;

namespace PhotinoX.App;

/// <summary>
/// A builder for PhotinoX cross-platform applications and services.
/// </summary>
public sealed class PhotinoAppBuilder
{
    private readonly ServiceCollection _services = [];
    private readonly Lazy<ConfigurationManager> _configuration;
    private readonly Lazy<PhotinoEnvironment> _environment;

    private bool _initializeAppServices;

    private Func<IServiceProvider>? _createServiceProvider;
    private Action<PhotinoApplication>? _configureApplication;
    private Func<IServiceProvider, PhotinoWindow>? _mainWindowFactory;

    internal PhotinoAppBuilder(PhotinoAppOptions appOptions, bool useDefaults = true)
    {
        ArgumentNullException.ThrowIfNull(appOptions);

        var configuration = new Lazy<ConfigurationManager>(static () => new ConfigurationManager());
        Services.AddSingleton<IConfiguration>(sp => configuration.Value);
        _configuration = configuration;

        var environment = new Lazy<PhotinoEnvironment>(() => CreateEnvironment(appOptions, useDefaults));
        Services.AddSingleton(sp => environment.Value);
        _environment = environment;

        _initializeAppServices = appOptions.InitializeAppServices;

        if (useDefaults)
        {
            this.UseDefaults(appOptions);
        }

        Debug.Assert(_configuration.IsValueCreated == useDefaults);
        Debug.Assert(_environment.IsValueCreated == false);
    }

    /// <summary>
    /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
    /// </summary>
    public IServiceCollection Services => _services;

    /// <summary>
    /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
    /// </summary>
    public ConfigurationManager Configuration => _configuration.Value;

    /// <summary>
    /// Information about the environment an application is running in.
    /// </summary>
    public PhotinoEnvironment Environment => _environment.Value;

    /// <summary>
    /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
    /// </summary>
    public ILoggingBuilder Logging
    {
        get
        {
            return field ??= InitializeLogging();

            ILoggingBuilder InitializeLogging()
            {
                // if someone accesses the Logging builder, ensure Logging has been initialized.
                Services.AddLogging();
                return new LoggingBuilder(Services);
            }
        }

        private set;
    }

    private PhotinoEnvironment CreateEnvironment(PhotinoAppOptions appOptions, bool useDefaults)
    {
        Debug.WriteLineIf(_configuration.IsValueCreated != useDefaults, "Configuration is instantiated");

        string contentRootPath, webRootPath;
        if (useDefaults)
        {
            contentRootPath = PathResolver.ResolveContentRootPath(
                !string.IsNullOrWhiteSpace(appOptions.ContentRootPath)
                    ? appOptions.ContentRootPath
                    : Configuration["PhotinoX:ContentRootPath"] ?? AppContext.BaseDirectory,
                AppContext.BaseDirectory);

            webRootPath = PathResolver.ResolveWebRootPath(
                !string.IsNullOrWhiteSpace(appOptions.WebRootPath)
                    ? appOptions.WebRootPath
                    : Configuration["PhotinoX:WebRootPath"],
                contentRootPath);
            return new PhotinoEnvironment
            {
                EnvironmentName = appOptions.GetEnvironmentName(Configuration["PhotinoX:EnvironmentName"] ?? "Production"),
                ApplicationName = appOptions.GetApplicationName(Configuration["PhotinoX:ApplicationName"] ?? "PhotinoX"),
                ContentRootPath = contentRootPath,
                WebRootPath = webRootPath
            };
        }

        contentRootPath = PathResolver.ResolveContentRootPath(appOptions.ContentRootPath, AppContext.BaseDirectory);
        webRootPath = PathResolver.ResolveWebRootPath(appOptions.WebRootPath, contentRootPath);

        return new PhotinoEnvironment
        {
            EnvironmentName = appOptions.GetEnvironmentName(),
            ApplicationName = appOptions.GetApplicationName(),
            ContentRootPath = contentRootPath,
            WebRootPath = webRootPath
        };
    }

    /// <summary>
    /// Configures the service provider factory used to create the application's root service provider.
    /// </summary>
    /// <typeparam name="TBuilder">The type of builder used by the service provider factory.</typeparam>
    /// <param name="factory">The service provider factory.</param>
    /// <param name="configure">An optional delegate used to configure the factory-specific builder.</param>
    /// <returns>The current <see cref="PhotinoAppBuilder"/>.</returns>
    public PhotinoAppBuilder ConfigureContainer<TBuilder>(IServiceProviderFactory<TBuilder> factory, Action<TBuilder>? configure = null) where TBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(factory);

        _createServiceProvider = () =>
        {
            var container = factory.CreateBuilder(Services);
            configure?.Invoke(container);
            return factory.CreateServiceProvider(container);
        };

        return this;
    }

    /// <summary>
    /// Configures the underlying <see cref="PhotinoApplication"/> before the application is built.
    /// </summary>
    /// <param name="configureApplication">A delegate used to configure the underlying application.</param>
    /// <returns>The current <see cref="PhotinoAppBuilder"/>.</returns>
    public PhotinoAppBuilder ConfigureApplication(Action<PhotinoApplication> configureApplication)
    {
        ArgumentNullException.ThrowIfNull(configureApplication);
        _configureApplication = configureApplication;
        return this;
    }

    /// <summary>
    /// Configures whether application initialization services are executed during build.
    /// </summary>
    /// <param name="enabled">
    /// <see langword="true"/> to execute registered <see cref="IPhotinoInitializeService"/> instances during build; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>The current <see cref="PhotinoAppBuilder"/>.</returns>
    public PhotinoAppBuilder UseAppServicesInitialization(bool enabled = true)
    {
        _initializeAppServices = enabled;
        return this;
    }

    /// <summary>
    /// Configures the factory used to create the application's main window.
    /// </summary>
    /// <param name="factory">
    /// A factory that creates the main <see cref="PhotinoWindow"/> using the application's root service provider.
    /// </param>
    /// <returns>The current <see cref="PhotinoAppBuilder"/>.</returns>
    public PhotinoAppBuilder UseMainWindow(Func<IServiceProvider, PhotinoWindow> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _mainWindowFactory = factory;
        return this;
    }

    /// <summary>
    /// Builds the <see cref="PhotinoApp"/>.
    /// </summary>
    /// <returns>A configured <see cref="PhotinoApp"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a <see cref="PhotinoApp"/> instance has already been created in the current process.
    /// </exception>
    public PhotinoApp Build()
    {
        ConfigureDefaultLogging();

        IServiceProvider serviceProvider = _createServiceProvider != null
            ? _createServiceProvider()
            : _services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = Environment.IsDevelopment,
                ValidateOnBuild = Environment.IsDevelopment
            });

        // Mark the service collection as read-only to prevent future modifications
        _services.MakeReadOnly();

        var appSettings = serviceProvider.GetService<IOptions<PhotinoAppSettings>>()?.Value;

        var application = new PhotinoApplication();
        if (!string.IsNullOrWhiteSpace(appSettings?.Runtime.WebView2RuntimePath))
        {
            application.SetWebView2RuntimePath(appSettings.Runtime.WebView2RuntimePath);
        }
        _configureApplication?.Invoke(application);

        var app = new PhotinoApp(serviceProvider, application, _mainWindowFactory);

        // Initialize application services that need access to the built root service provider.
        if (_initializeAppServices)
        {
            app.InitializeAppServices();
        }

        return app;
    }

    private void ConfigureDefaultLogging()
    {
        // By default, if no one else has configured logging, add a "no-op" LoggerFactory
        // and Logger services with no providers. This way when components try to get an
        // ILogger<> from the IServiceProvider, they don't get 'null'.
        Services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, NullLoggerFactory>());
        Services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(NullLogger<>)));
    }

    private sealed record LoggingBuilder(IServiceCollection Services) : ILoggingBuilder;
}
