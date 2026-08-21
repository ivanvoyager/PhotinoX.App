using System.Text.Json;
using Microsoft.Extensions.Logging;
using Photino.NET;

namespace PhotinoX.App.Dashboard;

public sealed class DashboardMessageHandler(
    DashboardSnapshotService snapshots,
    DashboardState state,
    ILogger<DashboardMessageHandler> logger)
{
    private PhotinoWindow? _detailsWindow;

    public void Handle(PhotinoApp app, PhotinoWindow window, string message)
    {
        try
        {
            using var document = JsonDocument.Parse(message);
            var type = document.RootElement.GetProperty("type").GetString();

            switch (type)
            {
                case "getSnapshot":
                    SendSnapshot(app, window);
                    break;

                case "closeWindow":
                    window.Close();
                    break;

                default:
                    SendError(window, $"Unknown message type: {type}");
                    break;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to handle dashboard message.");
            SendError(window, ex.Message);
        }
    }

    public void OpenDetailsWindow(PhotinoApp app)
    {
        if (_detailsWindow is { IsClosed: false })
        {
            _detailsWindow.BringToFront();
            return;
        }

        var detailsWindow = new PhotinoWindow()
            .ApplySettings(app.GetWindowConfiguration("Details"), app.Environment);

        detailsWindow.RegisterWebMessageReceivedHandler((sender, args) =>
        {
            if (sender is not PhotinoWindow sourceWindow)
                throw new InvalidOperationException("Web message sender is not a PhotinoWindow.");

            Handle(app, sourceWindow, args.Message);
        });

        detailsWindow.RegisterClosedHandler((_, _) =>
        {
            state.MarkDetailsWindowClosed();
            _detailsWindow = null;
        });

        _detailsWindow = detailsWindow;
        state.MarkDetailsWindowOpen();

        detailsWindow.Show();
    }

    private void SendSnapshot(PhotinoApp app, PhotinoWindow window)
    {
        var payload = new DashboardHostMessage(
            Type: "snapshot",
            Data: snapshots.CreateSnapshot(app));

        Send(window, payload);
    }

    private static void SendError(PhotinoWindow window, string message)
    {
        Send(window, new DashboardHostMessage(
            Type: "error",
            Error: message));
    }

    private static void Send(PhotinoWindow window, DashboardHostMessage payload)
    {
        var json = JsonSerializer.Serialize(
            payload,
            DashboardJsonContext.Default.DashboardHostMessage);

        window.SendWebMessage(json);
    }
}