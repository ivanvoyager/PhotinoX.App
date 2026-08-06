namespace PhotinoX.App;

/// <summary>
/// Provides extension methods for merging PhotinoX window configuration objects.
/// </summary>
public static class PhotinoWindowConfigurationMergeExtensions
{
    extension(PhotinoWindowConfiguration defaults)
    {
        /// <summary>
        /// Creates a new configuration by applying override values over default values.
        /// </summary>
        /// <param name="overrides">The override configuration.</param>
        /// <returns>The merged configuration.</returns>
        public PhotinoWindowConfiguration MergeWith(PhotinoWindowConfiguration overrides)
        {
            ArgumentNullException.ThrowIfNull(defaults);
            ArgumentNullException.ThrowIfNull(overrides);

            return new PhotinoWindowConfiguration
            {
                Window = defaults.Window.MergeWith(overrides.Window),
                Browser = defaults.Browser.MergeWith(overrides.Browser),
                Win32 = defaults.Win32.MergeWith(overrides.Win32),
                Linux = defaults.Linux.MergeWith(overrides.Linux)
            };
        }
    }

    extension(PhotinoWindowSettings defaults)
    {
        /// <summary>
        /// Creates a new window settings object by applying override values over default values.
        /// </summary>
        /// <param name="overrides">The override window settings.</param>
        /// <returns>The merged window settings.</returns>
        public PhotinoWindowSettings MergeWith(PhotinoWindowSettings overrides)
        {
            ArgumentNullException.ThrowIfNull(defaults);
            ArgumentNullException.ThrowIfNull(overrides);

            return new PhotinoWindowSettings
            {
                Title = overrides.Title ?? defaults.Title,
                Width = overrides.Width ?? defaults.Width,
                Height = overrides.Height ?? defaults.Height,
                Left = overrides.Left ?? defaults.Left,
                Top = overrides.Top ?? defaults.Top,
                CenterOnInitialize = overrides.CenterOnInitialize ?? defaults.CenterOnInitialize,
                UseOsDefaultSize = overrides.UseOsDefaultSize ?? defaults.UseOsDefaultSize,
                UseOsDefaultLocation = overrides.UseOsDefaultLocation ?? defaults.UseOsDefaultLocation,
                Resizable = overrides.Resizable ?? defaults.Resizable,
                Chromeless = overrides.Chromeless ?? defaults.Chromeless,
                Transparent = overrides.Transparent ?? defaults.Transparent,
                Topmost = overrides.Topmost ?? defaults.Topmost,
                StartUrl = overrides.StartUrl ?? defaults.StartUrl,
                StartString = overrides.StartString ?? defaults.StartString,
                IconFile = overrides.IconFile ?? defaults.IconFile
            };
        }
    }

    extension(PhotinoBrowserSettings defaults)
    {
        /// <summary>
        /// Creates a new browser settings object by applying override values over default values.
        /// </summary>
        /// <param name="overrides">The override browser settings.</param>
        /// <returns>The merged browser settings.</returns>
        public PhotinoBrowserSettings MergeWith(PhotinoBrowserSettings overrides)
        {
            ArgumentNullException.ThrowIfNull(defaults);
            ArgumentNullException.ThrowIfNull(overrides);

            return new PhotinoBrowserSettings
            {
                UserAgent = overrides.UserAgent ?? defaults.UserAgent,
                BrowserControlInitParameters = overrides.BrowserControlInitParameters ?? defaults.BrowserControlInitParameters,
                ContextMenuEnabled = overrides.ContextMenuEnabled ?? defaults.ContextMenuEnabled,
                ZoomEnabled = overrides.ZoomEnabled ?? defaults.ZoomEnabled,
                Zoom = overrides.Zoom ?? defaults.Zoom,
                DevToolsEnabled = overrides.DevToolsEnabled ?? defaults.DevToolsEnabled,
                GrantBrowserPermissions = overrides.GrantBrowserPermissions ?? defaults.GrantBrowserPermissions,
                MediaAutoplayEnabled = overrides.MediaAutoplayEnabled ?? defaults.MediaAutoplayEnabled,
                FileSystemAccessEnabled = overrides.FileSystemAccessEnabled ?? defaults.FileSystemAccessEnabled,
                WebSecurityEnabled = overrides.WebSecurityEnabled ?? defaults.WebSecurityEnabled,
                JavascriptClipboardAccessEnabled = overrides.JavascriptClipboardAccessEnabled ?? defaults.JavascriptClipboardAccessEnabled,
                MediaStreamEnabled = overrides.MediaStreamEnabled ?? defaults.MediaStreamEnabled,
                SmoothScrollingEnabled = overrides.SmoothScrollingEnabled ?? defaults.SmoothScrollingEnabled,
                IgnoreCertificateErrorsEnabled = overrides.IgnoreCertificateErrorsEnabled ?? defaults.IgnoreCertificateErrorsEnabled
            };
        }
    }

    extension(PhotinoWin32Settings defaults)
    {
        /// <summary>
        /// Creates a new Windows-specific settings object by applying override values over default values.
        /// </summary>
        /// <param name="overrides">The override Windows-specific settings.</param>
        /// <returns>The merged Windows-specific settings.</returns>
        public PhotinoWin32Settings MergeWith(PhotinoWin32Settings overrides)
        {
            ArgumentNullException.ThrowIfNull(defaults);
            ArgumentNullException.ThrowIfNull(overrides);

            return new PhotinoWin32Settings
            {
                UserDataFolder = overrides.UserDataFolder ?? defaults.UserDataFolder,
                NotificationsEnabled = overrides.NotificationsEnabled ?? defaults.NotificationsEnabled,
                NotificationRegistrationId = overrides.NotificationRegistrationId ?? defaults.NotificationRegistrationId,
                UseNativeWindowOwner = overrides.UseNativeWindowOwner ?? defaults.UseNativeWindowOwner
            };
        }
    }

    extension(PhotinoLinuxSettings defaults)
    {
        /// <summary>
        /// Creates a new Linux-specific settings object by applying override values over default values.
        /// </summary>
        /// <param name="overrides">The override Linux-specific settings.</param>
        /// <returns>The merged Linux-specific settings.</returns>
        public PhotinoLinuxSettings MergeWith(PhotinoLinuxSettings overrides)
        {
            ArgumentNullException.ThrowIfNull(defaults);
            ArgumentNullException.ThrowIfNull(overrides);

            return new PhotinoLinuxSettings
            {
                ChromelessDragRegionHeight = overrides.ChromelessDragRegionHeight ?? defaults.ChromelessDragRegionHeight,
                ChromelessDragRegionLeftInset = overrides.ChromelessDragRegionLeftInset ?? defaults.ChromelessDragRegionLeftInset,
                ChromelessDragRegionRightInset = overrides.ChromelessDragRegionRightInset ?? defaults.ChromelessDragRegionRightInset,
                ChromelessResizeBorderThickness = overrides.ChromelessResizeBorderThickness ?? defaults.ChromelessResizeBorderThickness
            };
        }
    }
}