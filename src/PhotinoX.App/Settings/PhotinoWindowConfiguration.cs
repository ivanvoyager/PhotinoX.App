namespace PhotinoX.App;

/// <summary>
/// Represents a complete PhotinoX window configuration.
/// </summary>
public sealed class PhotinoWindowConfiguration
{
    /// <summary>
    /// Gets or sets native window settings.
    /// </summary>
    public PhotinoWindowSettings Window { get; set; } = new();

    /// <summary>
    /// Gets or sets browser control settings.
    /// </summary>
    public PhotinoBrowserSettings Browser { get; set; } = new();

    /// <summary>
    /// Gets or sets Windows-specific settings.
    /// </summary>
    public PhotinoWin32Settings Win32 { get; set; } = new();

    /// <summary>
    /// Gets or sets Linux-specific settings.
    /// </summary>
    public PhotinoLinuxSettings Linux { get; set; } = new();
}