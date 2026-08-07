namespace PhotinoX.App;

/// <summary>
/// Represents PhotinoX application settings loaded from configuration.
/// </summary>
public sealed class PhotinoAppSettings
{
    /// <summary>
    /// Gets or sets the application name.
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// Gets or sets the application content root path.
    /// </summary>
    public string? ContentRootPath { get; set; }

    /// <summary>
    /// Gets or sets the web assets root path.
    /// </summary>
    public string? WebRootPath { get; set; }

    /// <summary>
    /// Gets or sets the default settings applied to configured windows.
    /// </summary>
    public PhotinoWindowConfiguration WindowDefaults { get; set; } = new();

    /// <summary>
    /// Gets or sets the main window configuration.
    /// </summary>
    public PhotinoWindowConfiguration MainWindow { get; set; } = new();

    /// <summary>
    /// Gets or sets named window configurations.
    /// </summary>
    public Dictionary<string, PhotinoWindowConfiguration> Windows { get; set; } = [];

    /// <summary>
    /// Gets or sets runtime-level settings.
    /// </summary>
    public PhotinoRuntimeSettings Runtime { get; set; } = new();
}