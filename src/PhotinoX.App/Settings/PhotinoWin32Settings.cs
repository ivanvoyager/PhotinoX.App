namespace PhotinoX.App;

/// <summary>
/// Represents Windows-specific PhotinoX settings.
/// </summary>
public sealed class PhotinoWin32Settings
{
    /// <summary>
    /// Gets or sets the WebView2 user data folder.
    /// </summary>
    public string? UserDataFolder { get; set; }

    /// <summary>
    /// Gets or sets whether native Windows notifications are enabled.
    /// </summary>
    public bool? NotificationsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the Windows notification registration identifier.
    /// </summary>
    public string? NotificationRegistrationId { get; set; }

    /// <summary>
    /// Gets or sets whether native owner-window behavior should be used for child windows.
    /// </summary>
    public bool? UseNativeWindowOwner { get; set; }
}