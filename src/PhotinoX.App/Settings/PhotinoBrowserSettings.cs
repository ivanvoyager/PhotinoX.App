namespace PhotinoX.App;

/// <summary>
/// Represents browser control settings.
/// </summary>
public sealed class PhotinoBrowserSettings
{
    /// <summary>
    /// Gets or sets the browser user agent.
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Gets or sets platform-specific browser initialization parameters.
    /// </summary>
    public string? BrowserControlInitParameters { get; set; }

    /// <summary>
    /// Gets or sets whether the default browser context menu is enabled.
    /// </summary>
    public bool? ContextMenuEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether browser zoom is enabled.
    /// </summary>
    public bool? ZoomEnabled { get; set; }

    /// <summary>
    /// Gets or sets the browser zoom level in percent.
    /// </summary>
    public int? Zoom { get; set; }

    /// <summary>
    /// Gets or sets whether browser developer tools are enabled.
    /// </summary>
    public bool? DevToolsEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether browser permission requests are granted automatically.
    /// </summary>
    public bool? GrantBrowserPermissions { get; set; }

    /// <summary>
    /// Gets or sets whether media autoplay is enabled.
    /// </summary>
    public bool? MediaAutoplayEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether local file system access is enabled.
    /// </summary>
    public bool? FileSystemAccessEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether browser web security is enabled.
    /// </summary>
    public bool? WebSecurityEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether JavaScript clipboard access is enabled.
    /// </summary>
    public bool? JavascriptClipboardAccessEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether media stream access is enabled.
    /// </summary>
    public bool? MediaStreamEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether smooth scrolling is enabled.
    /// </summary>
    public bool? SmoothScrollingEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether browser certificate errors are ignored.
    /// </summary>
    public bool? IgnoreCertificateErrorsEnabled { get; set; }
}