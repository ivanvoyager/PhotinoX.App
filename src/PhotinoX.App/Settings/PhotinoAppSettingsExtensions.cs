using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace PhotinoX.App;

/// <summary>
/// Provides extension methods for reading PhotinoX application settings.
/// </summary>
public static class PhotinoAppSettingsExtensions
{
    extension(PhotinoApp app)
    {
        /// <summary>
        /// Gets the effective main window configuration.
        /// </summary>
        /// <returns>The effective main window configuration.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the application is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="PhotinoAppSettings"/> has not been registered.
        /// </exception>
        public PhotinoWindowConfiguration GetMainWindowConfiguration()
        {
            ArgumentNullException.ThrowIfNull(app);

            var settings = app.Services.GetService<IOptions<PhotinoAppSettings>>()?.Value
                ?? throw new InvalidOperationException($"{nameof(PhotinoAppSettings)} is not registered.");

            return settings.WindowDefaults.MergeWith(settings.MainWindow);
        }

        /// <summary>
        /// Gets the effective named window configuration.
        /// </summary>
        /// <param name="name">The named window configuration name.</param>
        /// <returns>The effective named window configuration.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the application or <paramref name="name"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="name"/> is empty or whitespace.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="PhotinoAppSettings"/> has not been registered.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the named window configuration is not found.
        /// </exception>
        public PhotinoWindowConfiguration GetWindowConfiguration(string name)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var settings = app.Services.GetService<IOptions<PhotinoAppSettings>>()?.Value
                ?? throw new InvalidOperationException($"{nameof(PhotinoAppSettings)} is not registered.");

            if (!settings.Windows.TryGetValue(name, out var configuration))
                throw new KeyNotFoundException($"Window configuration '{name}' was not found.");

            return settings.WindowDefaults.MergeWith(configuration);
        }
    }
}