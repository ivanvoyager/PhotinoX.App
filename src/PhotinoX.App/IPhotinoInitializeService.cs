namespace PhotinoX.App;

/// <summary>
/// Represents a service that is initialized during the application construction.
/// </summary>
/// <remarks>
/// This service is initialized during the <see cref="PhotinoAppBuilder.Build()"/> method. It is
/// executed once per application using the root service provider.
/// </remarks>
public interface IPhotinoInitializeService
{
    void Initialize(IServiceProvider services);
}
