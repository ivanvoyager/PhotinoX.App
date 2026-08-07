using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PhotinoX.App.Dashboard;

public sealed class DashboardInitializer : IPhotinoInitializeService
{
    public void Initialize(IServiceProvider services)
    {
        var state = services.GetRequiredService<DashboardState>();
        var logger = services.GetRequiredService<ILogger<DashboardInitializer>>();
        var environment = services.GetRequiredService<PhotinoEnvironment>();

        state.MarkInitialized();

        logger.LogInformation(
            "Dashboard initialized for {ApplicationName}. Web root: {WebRootPath}",
            environment.ApplicationName,
            environment.WebRootPath);
    }
}