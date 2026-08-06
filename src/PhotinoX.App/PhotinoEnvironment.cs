namespace PhotinoX.App;

/// <summary>
/// Provides information about the PhotinoX application environment.
/// </summary>
public sealed class PhotinoEnvironment
{
    /// <summary>
    /// Gets the application environment name.
    /// </summary>
    public string EnvironmentName { get; init; } = null!;

    /// <summary>
    /// Gets the application name.
    /// </summary>
    public string ApplicationName { get; init; } = null!;

    /// <summary>
    /// Gets the application content root path.
    /// </summary>
    public string ContentRootPath { get; init; } = null!;

    /// <summary>
    /// Gets the web assets root path.
    /// </summary>
    public string WebRootPath { get; init; } = null!;

    /// <summary>
    /// Gets a value indicating whether the current environment is Development.
    /// </summary>
    public bool IsDevelopment => string.Equals(EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the current environment is Production.
    /// </summary>
    public bool IsProduction => string.Equals(EnvironmentName, "Production", StringComparison.OrdinalIgnoreCase);
}