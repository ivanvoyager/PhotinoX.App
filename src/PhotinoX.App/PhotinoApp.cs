using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Photino.NET;

namespace PhotinoX.App;

/// <summary>
/// A .NET PhotinoX application with registered services and configuration data.
/// </summary>
/// <remarks>
/// <see cref="PhotinoApp"/> provides the high-level application composition layer for
/// dependency injection, configuration, startup services, and main-window creation.
/// The native application lifetime and message loop are provided by the underlying
/// <see cref="PhotinoApplication"/>.
/// </remarks>
public sealed class PhotinoApp : IDisposable, IAsyncDisposable
{
    private readonly Func<PhotinoApp, PhotinoWindow>? _mainWindowFactory;
    private int _disposed;
    private int _appServicesInitialized;

    private static int s_appCreated;

    internal PhotinoApp(
        IServiceProvider services,
        PhotinoApplication application,
        Func<PhotinoApp, PhotinoWindow>? mainWindowFactory = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(application);

        if (Interlocked.CompareExchange(ref s_appCreated, 1, 0) == 1)
            throw new InvalidOperationException($"Cannot create more than one {typeof(PhotinoApp).FullName} instance.");

        Services = services;
        Application = application;
        _mainWindowFactory = mainWindowFactory;

        Current = this;
    }

    /// <summary>
    /// Gets the current <see cref="PhotinoApp"/> instance.
    /// </summary>
    /// <remarks>
    /// Only one <see cref="PhotinoApp"/> instance can be created in a process.
    /// </remarks>
    public static PhotinoApp Current { get; private set; } = null!;

    /// <summary>
    /// Gets the application's configured services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Gets the application's configured <see cref="IConfiguration"/>.
    /// </summary>
    public IConfiguration Configuration => Services.GetRequiredService<IConfiguration>();

    /// <summary>
    /// Gets information about the application's environment.
    /// </summary>
    public PhotinoEnvironment Environment => Services.GetRequiredService<PhotinoEnvironment>();

    internal PhotinoApplication Application { get; }

    /// <summary>
    /// Gets the dispatcher associated with the underlying Photino application.
    /// </summary>
    public PhotinoDispatcher Dispatcher => Application.Dispatcher;

    /// <summary>
    /// Gets the application's main window.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the main window has not been created yet.
    /// </exception>
    public PhotinoWindow MainWindow
    {
        get => field ?? throw new InvalidOperationException("MainWindow is not created yet.");
        private set;
    }

    private PhotinoWindow CreateMainWindow()
    {
        ThrowIfDisposed();

        if (_mainWindowFactory is null)
            throw new InvalidOperationException("No main window configured.");

        return _mainWindowFactory(this);
    }

    /// <summary>
    /// Initializes registered application initialization services using the root service provider.
    /// </summary>
    /// <remarks>
    /// Registered <see cref="IPhotinoInitializeService"/> instances are executed at most once per application.
    /// </remarks>
    public void InitializeAppServices()
    {
        ThrowIfDisposed();

        if (Interlocked.CompareExchange(ref _appServicesInitialized, 1, 0) != 0)
            return;

        try
        {
            var initServices = Services.GetServices<IPhotinoInitializeService>();

            foreach (var instance in initServices)
                instance.Initialize(Services);
        }
        catch
        {
            Volatile.Write(ref _appServicesInitialized, 0);
            throw;
        }
    }

    /// <summary>
    /// Runs the application with the specified main window, or creates one using the configured main-window factory.
    /// </summary>
    /// <param name="mainWindow">
    /// The main window to run, or <see langword="null"/> to use the configured main-window factory.
    /// </param>
    /// <returns>
    /// The application exit code.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown when the application has already been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <paramref name="mainWindow"/> is <see langword="null"/> and no main-window factory has been configured.
    /// </exception>
    public int Run(PhotinoWindow? mainWindow = null)
    {
        ThrowIfDisposed();

        try
        {
            mainWindow ??= CreateMainWindow();

            MainWindow = mainWindow;

            return Application.Run(mainWindow);
        }
        finally
        {
            Dispose();
        }
    }

    /// <summary>
    /// Releases the resources used by the application.
    /// </summary>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return;

        (Services as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Asynchronously releases the resources used by the application.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous dispose operation.
    /// </returns>
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
            return;

        if (Services is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        else
            (Services as IDisposable)?.Dispose();
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
    }
}