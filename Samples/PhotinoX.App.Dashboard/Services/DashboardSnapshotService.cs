using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using PhotinoX.App;

namespace PhotinoX.App.Dashboard;

public sealed class DashboardSnapshotService
{
    private readonly PhotinoEnvironment _environment;
    private readonly DashboardState _state;
    private readonly IOptions<PhotinoAppSettings> _settings;

    private readonly Process _process = Process.GetCurrentProcess();

    private DateTimeOffset _lastCpuSampleAt = DateTimeOffset.Now;
    private TimeSpan _lastTotalProcessorTime;
    private double _lastCpuUsage;

    public DashboardSnapshotService(
        PhotinoEnvironment environment,
        DashboardState state,
        IOptions<PhotinoAppSettings> settings)
    {
        _environment = environment;
        _state = state;
        _settings = settings;
        _lastTotalProcessorTime = _process.TotalProcessorTime;
    }

    public DashboardSnapshot CreateSnapshot(PhotinoApp app)
    {
        _process.Refresh();

        var now = DateTimeOffset.Now;
        var managedMemory = GC.GetTotalMemory(false);
        var privateMemory = _process.PrivateMemorySize64;
        var nativeMemory = Math.Max(0, privateMemory - managedMemory);

        return new DashboardSnapshot(
            Application: new DashboardApplicationSnapshot(
                Name: _environment.ApplicationName,
                EnvironmentName: _environment.EnvironmentName,
                InitializedAt: FormatDateTime(_state.InitializedAt),
                Status: _state.InitializedAt is null ? "Starting" : "Ready"),
            Platform: new DashboardPlatformSnapshot(
                OsDescription: GetShortOsDescription(),
                ProcessArchitecture: RuntimeInformation.ProcessArchitecture.ToString(),
                DotnetVersion: Environment.Version.ToString()),
            Runtime: new DashboardRuntimeSnapshot(
                CurrentTime: now.ToString("HH:mm:ss"),
                Uptime: FormatDuration(now - _state.StartedAt),
                ManagedMemory: FormatBytes(managedMemory),
                NativeMemory: FormatBytes(nativeMemory),
                PrivateMemory: FormatBytes(privateMemory),
                CpuUsage: FormatCpuUsage(),
                ThreadPoolWorkers: FormatThreadPoolWorkers(),
                ProcessThreadCount: GetProcessThreadCount(),
                GcGen0: GC.CollectionCount(0),
                GcGen1: GC.CollectionCount(1),
                GcGen2: GC.CollectionCount(2)),
            Configuration: new DashboardConfigurationSnapshot(
                ContentRootPath: _environment.ContentRootPath,
                WebRootPath: _environment.WebRootPath,
                MainWindow: "WindowDefaults + MainWindow",
                DetailsWindow: _settings.Value.Windows.ContainsKey("Details")
                    ? "WindowDefaults + Windows[\"Details\"]"
                    : "Not configured"),
            Windows: CreateWindowSnapshots(app));
    }

    private DashboardWindowSnapshot[] CreateWindowSnapshots(PhotinoApp app)
    {
        var mainWindow = app.MainWindow;

        return
        [
            new DashboardWindowSnapshot(
                Name: "Main",
                Status: mainWindow.IsClosed ? "Closed" : "Open",
                Title: GetMainWindowTitle(),
                IsMain: true),

            new DashboardWindowSnapshot(
                Name: "Details",
                Status: _state.DetailsWindowStatus,
                Title: GetNamedWindowTitle("Details"),
                IsMain: false)
        ];
    }

    private string GetMainWindowTitle()
    {
        return _settings.Value.MainWindow.Window.Title
            ?? _settings.Value.ApplicationName
            ?? _environment.ApplicationName;
    }

    private string GetNamedWindowTitle(string name)
    {
        return _settings.Value.Windows.TryGetValue(name, out var configuration)
            ? configuration.Window.Title ?? name
            : name;
    }

    private string FormatCpuUsage()
    {
        var now = DateTimeOffset.Now;
        var totalProcessorTime = _process.TotalProcessorTime;

        var processorDelta = totalProcessorTime - _lastTotalProcessorTime;
        var elapsed = now - _lastCpuSampleAt;

        if (elapsed.TotalMilliseconds > 0)
        {
            _lastCpuUsage = processorDelta.TotalMilliseconds / elapsed.TotalMilliseconds / Environment.ProcessorCount * 100;
            _lastCpuSampleAt = now;
            _lastTotalProcessorTime = totalProcessorTime;
        }

        return $"{_lastCpuUsage:0.0}%";
    }

    private static string FormatThreadPoolWorkers()
    {
        ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out _);
        ThreadPool.GetMaxThreads(out var maxWorkerThreads, out _);

        var busyWorkerThreads = Math.Max(0, maxWorkerThreads - availableWorkerThreads);

        return $"{busyWorkerThreads}/{maxWorkerThreads}";
    }

    private int GetProcessThreadCount()
    {
        try
        {
            return _process.Threads.Count;
        }
        catch
        {
            return 0;
        }
    }

    private static string GetShortOsDescription()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return $"Windows {Environment.OSVersion.Version}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return $"macOS {Environment.OSVersion.Version}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return RuntimeInformation.OSDescription.Replace("GNU/Linux", "Linux", StringComparison.OrdinalIgnoreCase);

        return RuntimeInformation.OSDescription;
    }

    private static string FormatDateTime(DateTimeOffset? value)
    {
        return value?.ToString("HH:mm:ss") ?? "-";
    }

    private static string FormatDuration(TimeSpan value)
    {
        return value.TotalHours >= 1
            ? value.ToString(@"hh\:mm\:ss")
            : value.ToString(@"mm\:ss");
    }

    private static string FormatBytes(long bytes)
    {
        const double kb = 1024;
        const double mb = kb * 1024;
        const double gb = mb * 1024;

        return bytes switch
        {
            >= (long)gb => $"{bytes / gb:0.0} GB",
            >= (long)mb => $"{bytes / mb:0.1} MB",
            >= (long)kb => $"{bytes / kb:0.1} KB",
            _ => $"{bytes} B"
        };
    }
}