namespace PhotinoX.App.Dashboard;

public sealed record DashboardHostMessage(
    string Type,
    DashboardSnapshot? Data = null,
    string? Error = null,
    string? Message = null);

public sealed record DashboardSnapshot(
    DashboardApplicationSnapshot Application,
    DashboardPhotinoRuntimeSnapshot PhotinoRuntime,
    DashboardRuntimeSnapshot Runtime,
    DashboardConfigurationSnapshot Configuration,
    DashboardWindowSnapshot[] Windows);

public sealed record DashboardApplicationSnapshot(
    string Name,
    string EnvironmentName,
    string InitializedAt,
    string Status);

public sealed record DashboardPhotinoRuntimeSnapshot(
    string NativeVersion,
    string WebViewEngine,
    string WebViewRuntimeVersion,
    string OSDescription,
    string OSArchitecture,
    string ProcessArchitecture,
    string FrameworkDescription,
    DashboardRuntimeInfoItem[] PlatformDetails);

public sealed record DashboardRuntimeInfoItem(
    string Name,
    string Value);

public sealed record DashboardRuntimeSnapshot(
    string CurrentTime,
    string Uptime,
    string ManagedMemory,
    string NativeMemory,
    string PrivateMemory,
    string CpuUsage,
    string ThreadPoolWorkers,
    int ProcessThreadCount,
    int GcGen0,
    int GcGen1,
    int GcGen2);

public sealed record DashboardConfigurationSnapshot(
    string ContentRootPath,
    string WebRootPath,
    string MainWindow,
    string DetailsWindow);

public sealed record DashboardWindowSnapshot(
    string Name,
    string Status,
    string Title,
    bool IsMain);