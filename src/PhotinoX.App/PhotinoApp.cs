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
public sealed partial class PhotinoApp : IDisposable, IAsyncDisposable
{
    private static class States
    {
        public const int NotDisposed = 0;// default value of _state
        public const int Disposing = 1;
        public const int Disposed = 2;
    }

    private readonly Func<PhotinoApp, PhotinoWindow>? _mainWindowFactory;
    private readonly Action<PhotinoApp>? _beforeDispose;
    private int _isRunning;
    private int _state;
    private int _appServicesInitialized;

    private static int s_appCreated;

    internal PhotinoApp(
        IServiceProvider services,
        PhotinoApplication application,
        Func<PhotinoApp, PhotinoWindow>? mainWindowFactory = null,
        Action<PhotinoApp>? beforeDispose = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(application);

        if (Interlocked.CompareExchange(ref s_appCreated, 1, 0) == 1)
            ThrowApplicationAlreadyCreated();

        Services = services;
        Application = application;
        _mainWindowFactory = mainWindowFactory;
        _beforeDispose = beforeDispose;

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

    /// <summary>
    /// Gets the underlying Photino application.
    /// </summary>
    public PhotinoApplication Application { get; }

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
    public PhotinoWindow MainWindow => Application.MainWindow ?? ThrowMainWindowNotCreated();

    private PhotinoWindow CreateMainWindow()
    {
        ThrowIfDisposingOrDisposed();

        if (_mainWindowFactory is null)
            ThrowMainWindowNotConfigured();

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
        ThrowIfDisposingOrDisposed();

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
        ThrowIfDisposingOrDisposed();

        if (Interlocked.CompareExchange(ref _isRunning, 1, 0) != 0)
            ThrowApplicationAlreadyRunning();

        try
        {
            mainWindow ??= CreateMainWindow();
            return Application.Run(mainWindow);
        }
        finally
        {
            Volatile.Write(ref _isRunning, 0);
        }
    }

    /// <summary>
    /// Releases the resources used by the application.
    /// </summary>
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _state, States.Disposing, States.NotDisposed) != States.NotDisposed)
            return;

        try
        {
            try
            {
                _beforeDispose?.Invoke(this);
            }
            finally
            {
                (Services as IDisposable)?.Dispose();
            }
        }
        finally
        {
            Volatile.Write(ref _state, States.Disposed);
        }
    }

    /// <summary>
    /// Asynchronously releases the resources used by the application.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous dispose operation.
    /// </returns>
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.CompareExchange(ref _state, States.Disposing, States.NotDisposed) != States.NotDisposed)
            return;

        try
        {
            try
            {
                _beforeDispose?.Invoke(this);
            }
            finally
            {
                if (Services is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                else
                    (Services as IDisposable)?.Dispose();
            }
        }
        finally
        {
            Volatile.Write(ref _state, States.Disposed);
        }
    }

    internal void ResetCurrent()
    {
        Application.ResetCurrent();

        if (ReferenceEquals(Current, this))
            Current = null!;

        Volatile.Write(ref s_appCreated, 0);
    }
}