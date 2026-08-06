namespace PhotinoX.App;

/// <summary>
/// Options for configuring the behavior for <see cref="PhotinoAppExtensions.CreateBuilder(PhotinoAppOptions)"/>.
/// </summary>
public class PhotinoAppOptions
{
    /// <summary>
    /// The command line arguments.
    /// </summary>
    public string[]? Args { get; init; }

    /// <summary>
    /// The environment name.
    /// </summary>
    public string? EnvironmentName { get; init; }

    /// <summary>
    /// The application name.
    /// </summary>
    public string? ApplicationName { get; init; }

    /// <summary>
    /// The content root path.
    /// </summary>
    public string? ContentRootPath { get; init; }

    /// <summary>
    /// The web root path.
    /// </summary>
    public string? WebRootPath { get; init; }

    /// <summary>
    /// Gets or sets whether registered application initialization services should be executed during build.
    /// </summary>
    public bool InitializeAppServices { get; init; } = true;
}