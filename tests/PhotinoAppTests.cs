using Microsoft.Extensions.DependencyInjection;
using Photino.NET;

namespace PhotinoX.App.Tests;

[TestClass]
public sealed class PhotinoAppTests
{
    [TestMethod]
    public void Application_ReturnsConfiguredApplication()
    {
        PhotinoApplication? configuredApplication = null;

        using var app = PhotinoApp
            .CreateBuilder(useDefaults: false)
            .ConfigureApplication(application =>
            {
                configuredApplication = application;
            })
            .ConfigureBeforeDispose(app => app.ResetCurrent())
            .Build();

        Assert.IsNotNull(configuredApplication);
        Assert.AreSame(configuredApplication, app.Application);
    }

    [TestMethod]
    public void Dispose_InvokesBeforeDisposeBeforeDisposingServices()
    {
        var events = new List<string>();

        var builder = PhotinoApp.CreateBuilder(useDefaults: false);

        builder.Services.AddSingleton(_ => new TrackedService(() => events.Add("ServicesDisposed")));
        builder.ConfigureBeforeDispose(_ => events.Add("BeforeDispose"));

        var app = builder
            .ConfigureBeforeDispose(app => app.ResetCurrent())
            .Build();

        _ = app.Services.GetRequiredService<TrackedService>();

        app.Dispose();

        Assert.AreSequenceEqual(["BeforeDispose", "ServicesDisposed"], events);
    }

    private sealed class TrackedService(Action onDispose) : IDisposable
    {
        public void Dispose()
        {
            onDispose();
        }
    }
}