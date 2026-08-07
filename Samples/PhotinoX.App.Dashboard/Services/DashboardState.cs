namespace PhotinoX.App.Dashboard;

public sealed class DashboardState
{
    public DateTimeOffset StartedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset? InitializedAt { get; private set; }

    public string DetailsWindowStatus { get; private set; } = "Configured";

    public void MarkInitialized()
    {
        InitializedAt = DateTimeOffset.Now;
    }

    public void MarkDetailsWindowOpen()
    {
        DetailsWindowStatus = "Open";
    }

    public void MarkDetailsWindowClosed()
    {
        DetailsWindowStatus = "Closed";
    }
}