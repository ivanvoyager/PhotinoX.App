using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using PhotinoX.App;
using PhotinoX.App.Dashboard;

var builder = PhotinoApp.CreateBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddSingleton<DashboardState>();
    services.AddSingleton<DashboardSnapshotService>();
    services.AddSingleton<DashboardMessageHandler>();
    services.AddSingleton<IPhotinoInitializeService, DashboardInitializer>();
});

builder.UseMainWindow(app =>
{
    var window = new PhotinoWindow()
        .ApplySettings(app.GetMainWindowConfiguration(), app.Environment);

    window.WebMessageReceived += OnWebMessageReceived;
    window.NewWindowRequested += OnNewWindowRequested;

    return window;

    void OnWebMessageReceived(object? sender, WebMessageReceivedEventArgs args)
    {
        if (sender is not PhotinoWindow sourceWindow)
            throw new InvalidOperationException("Web message sender is not a PhotinoWindow.");
        var handler = app.Services.GetRequiredService<DashboardMessageHandler>();
        handler.Handle(app, sourceWindow, args.Message);
    }

    void OnNewWindowRequested(object? sender, NewWindowRequestedEventArgs args)
    {
        var handler = app.Services.GetRequiredService<DashboardMessageHandler>();
        if (Path.GetFileName(args.Uri.AbsolutePath).Equals("details.html"))
        {
            handler.OpenDetailsWindow(app);
        }
    }
});

builder.ConfigureApplication(application =>
{
    application.ShutdownMode = PhotinoShutdownMode.OnMainWindowClose;
});

using var app = builder.Build();

return app.Run();