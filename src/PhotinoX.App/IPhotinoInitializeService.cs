namespace PhotinoX.App;

/// <summary>
/// Represents a service that is initialized after the application's root service provider has been built.
/// </summary>
/// <remarks>
/// Registered services are initialized by <see cref="PhotinoApp.InitializeAppServices"/>.
/// By default, <see cref="PhotinoAppBuilder.Build"/> calls that method automatically.
/// </remarks>
public interface IPhotinoInitializeService
{
    /// <summary>
    /// Initializes the service using the application's root service provider.
    /// </summary>
    /// <param name="services">The application's root service provider.</param>
    void Initialize(IServiceProvider services);
}