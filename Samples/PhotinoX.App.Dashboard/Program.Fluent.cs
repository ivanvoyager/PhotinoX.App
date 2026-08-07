using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using PhotinoX.App;
using PhotinoX.App.Dashboard;

return PhotinoApp.CreateBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<DashboardState>();
        services.AddSingleton<DashboardSnapshotService>();
        services.AddSingleton<DashboardMessageHandler>();
        services.AddSingleton<IPhotinoInitializeService, DashboardInitializer>();
    })
    .UseMainWindow(app =>
    {
        return new PhotinoWindow()
            .ApplySettings(app.GetMainWindowConfiguration(), app.Environment)
            .RegisterWebMessageReceivedHandler((sender, args) =>
            {
                if (sender is not PhotinoWindow sourceWindow)
                    throw new InvalidOperationException("Web message sender is not a PhotinoWindow.");

                var handler = app.Services.GetRequiredService<DashboardMessageHandler>();
                handler.Handle(app, sourceWindow, args.Message);
            })
            .RegisterNewWindowRequestedHandler((_, args) =>
            {
                if (Path.GetFileName(args.Uri.AbsolutePath).Equals("details.html"))
                {
                    var handler = app.Services.GetRequiredService<DashboardMessageHandler>();
                    handler.OpenDetailsWindow(app);
                }
            });
    })
    .ConfigureApplication(application =>
    {
        application.ShutdownMode = PhotinoShutdownMode.OnMainWindowClose;
    })
    .Build()
    .Run();