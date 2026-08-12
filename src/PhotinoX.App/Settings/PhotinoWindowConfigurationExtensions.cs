using Photino.NET;

namespace PhotinoX.App;

/// <summary>
/// Provides extension methods for applying PhotinoX configuration settings to windows.
/// </summary>
public static class PhotinoWindowConfigurationExtensions
{
    extension(PhotinoWindow window)
    {
        /// <summary>
        /// Applies the configured main window settings to the current <see cref="PhotinoWindow"/>,
        /// including default window settings.
        /// </summary>
        /// <param name="settings">The application settings to apply.</param>
        /// <param name="environment">The application environment used to resolve relative web asset paths.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="settings"/> or <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        public PhotinoWindow ApplyMainWindowSettings(PhotinoAppSettings settings, PhotinoEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(environment);

            return window.ApplySettings(settings.WindowDefaults.MergeWith(settings.MainWindow), environment);
        }

        /// <summary>
        /// Applies the configured named window settings to the current <see cref="PhotinoWindow"/>,
        /// including default window settings.
        /// </summary>
        /// <param name="settings">The application settings to apply.</param>
        /// <param name="name">The named window configuration to apply.</param>
        /// <param name="environment">The application environment used to resolve relative web asset paths.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="settings"/>, <paramref name="name"/>, or <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="name"/> is empty or whitespace.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the named window configuration is not found.
        /// </exception>
        public PhotinoWindow ApplyWindowSettings(PhotinoAppSettings settings, string name, PhotinoEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNull(environment);

            if (!settings.Windows.TryGetValue(name, out var configuration))
                throw new KeyNotFoundException($"Window configuration '{name}' was not found.");

            return window.ApplySettings(settings.WindowDefaults.MergeWith(configuration), environment);
        }

        /// <summary>
        /// Applies the specified complete window configuration to the current <see cref="PhotinoWindow"/>.
        /// </summary>
        /// <param name="configuration">The window configuration to apply.</param>
        /// <param name="environment">The application environment used to resolve relative web asset paths.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="configuration"/> or <paramref name="environment"/> is <see langword="null"/>.
        /// </exception>
        public PhotinoWindow ApplySettings(PhotinoWindowConfiguration configuration, PhotinoEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(environment);

            window.ApplySettings(configuration.Window, environment);
            window.ApplySettings(configuration.Browser);
            window.ApplySettings(configuration.Win32);
            window.ApplySettings(configuration.Linux);

            return window;
        }

        /// <summary>
        /// Applies the specified window settings to the current <see cref="PhotinoWindow"/>.
        /// </summary>
        /// <param name="settings">The settings to apply.</param>
        /// <param name="environment">The application environment used to resolve relative web asset paths.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="settings"/> is <see langword="null"/>.
        /// </exception>
        public PhotinoWindow ApplySettings(PhotinoWindowSettings settings, PhotinoEnvironment? environment = null)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(settings);

            if (settings.Title is not null)
                window.SetTitle(settings.Title);

            if (settings.UseOsDefaultSize is { } useOsDefaultSize)
                window.UseOsDefaultSize = useOsDefaultSize;

            if (settings.UseOsDefaultLocation is { } useOsDefaultLocation)
                window.UseOsDefaultLocation = useOsDefaultLocation;

            if (settings.Width is { } width)
                window.SetWidth(width);

            if (settings.Height is { } height)
                window.SetHeight(height);

            if (settings.Left is { } left)
                window.SetLeft(left);

            if (settings.Top is { } top)
                window.SetTop(top);

            if (settings.CenterOnInitialize is { } center)
                window.CenterOnInitialize = center;

            if (settings.Resizable is { } resizable)
                window.SetResizable(resizable);

            if (settings.Chromeless is { } chromeless)
                window.SetChromeless(chromeless);

            if (settings.Transparent is { } transparent)
                window.SetTransparent(transparent);

            if (settings.Topmost is { } topmost)
                window.SetTopmost(topmost);

            if (settings.IconFile is not null)
                window.SetIconFile(settings.IconFile);

            if (settings.StartString is not null)
            {
                window.LoadString(settings.StartString);
            }

            if (settings.StartUrl is not null)
            {
                window.Load(environment is null ? settings.StartUrl : environment.ResolveStartUrl(settings.StartUrl));
            }

            return window;
        }

        /// <summary>
        /// Applies the specified browser settings to the current <see cref="PhotinoWindow"/>.
        /// </summary>
        /// <param name="settings">The browser settings to apply.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="settings"/> is <see langword="null"/>.
        /// </exception>
        public PhotinoWindow ApplySettings(PhotinoBrowserSettings settings)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(settings);

            if (settings.UserAgent is not null)
                window.SetUserAgent(settings.UserAgent);

            if (settings.BrowserControlInitParameters is not null)
                window.SetBrowserControlInitParameters(settings.BrowserControlInitParameters);

            if (settings.ContextMenuEnabled is { } contextMenuEnabled)
                window.SetContextMenuEnabled(contextMenuEnabled);

            if (settings.ZoomEnabled is { } zoomEnabled)
                window.SetZoomEnabled(zoomEnabled);

            if (settings.Zoom is { } zoom)
                window.SetZoom(zoom);

            if (settings.DevToolsEnabled is { } devToolsEnabled)
                window.SetDevToolsEnabled(devToolsEnabled);

            if (settings.GrantBrowserPermissions is { } grantBrowserPermissions)
                window.SetGrantBrowserPermissions(grantBrowserPermissions);

            if (settings.MediaAutoplayEnabled is { } mediaAutoplayEnabled)
                window.SetMediaAutoplayEnabled(mediaAutoplayEnabled);

            if (settings.FileSystemAccessEnabled is { } fileSystemAccessEnabled)
                window.SetFileSystemAccessEnabled(fileSystemAccessEnabled);

            if (settings.WebSecurityEnabled is { } webSecurityEnabled)
                window.SetWebSecurityEnabled(webSecurityEnabled);

            if (settings.JavascriptClipboardAccessEnabled is { } javascriptClipboardAccessEnabled)
                window.SetJavascriptClipboardAccessEnabled(javascriptClipboardAccessEnabled);

            if (settings.MediaStreamEnabled is { } mediaStreamEnabled)
                window.SetMediaStreamEnabled(mediaStreamEnabled);

            if (settings.SmoothScrollingEnabled is { } smoothScrollingEnabled)
                window.SetSmoothScrollingEnabled(smoothScrollingEnabled);

            if (settings.IgnoreCertificateErrorsEnabled is { } ignoreCertificateErrorsEnabled)
                window.SetIgnoreCertificateErrorsEnabled(ignoreCertificateErrorsEnabled);

            return window;
        }

        /// <summary>
        /// Applies the specified Windows-specific settings to the current <see cref="PhotinoWindow"/>.
        /// </summary>
        /// <param name="settings">The Windows-specific settings to apply.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="settings"/> is <see langword="null"/>.
        /// </exception>
        public PhotinoWindow ApplySettings(PhotinoWin32Settings settings)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(settings);

            if (settings.UserDataFolder is not null)
                window.SetUserDataFolder(settings.UserDataFolder);

            if (settings.UseNativeWindowOwner is { } useNativeWindowOwner)
                window.SetUseNativeWindowOwner(useNativeWindowOwner);

            return window;
        }

        /// <summary>
        /// Applies the specified Linux-specific settings to the current <see cref="PhotinoWindow"/>.
        /// </summary>
        /// <param name="settings">The Linux-specific settings to apply.</param>
        /// <returns>The current <see cref="PhotinoWindow"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="settings"/> is <see langword="null"/>.
        /// </exception>
        public PhotinoWindow ApplySettings(PhotinoLinuxSettings settings)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(settings);

            if (settings.ChromelessDragRegionHeight is not null ||
                settings.ChromelessDragRegionLeftInset is not null ||
                settings.ChromelessDragRegionRightInset is not null)
            {
                var current = window.LinuxChromelessSettings;

                window.SetLinuxChromelessDragRegion(
                    settings.ChromelessDragRegionHeight ?? current.DragRegionHeight,
                    settings.ChromelessDragRegionRightInset ?? current.DragRegionRightInset,
                    settings.ChromelessDragRegionLeftInset ?? current.DragRegionLeftInset);
            }

            if (settings.ChromelessResizeBorderThickness is { } resizeBorderThickness)
                window.SetLinuxChromelessResizeBorderThickness(resizeBorderThickness);

            return window;
        }
    }
}