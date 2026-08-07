using Microsoft.Extensions.DependencyInjection;

namespace PhotinoX.App;

/// <summary>
/// Provides factory and initialization extension methods for <see cref="PhotinoApp"/>.
/// </summary>
public static class PhotinoAppExtensions
{
    extension (PhotinoApp app)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhotinoApp"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="PhotinoApp"/>.</returns>
        public static PhotinoApp Create(string[]? args = null) =>
            new PhotinoAppBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Creates a new <see cref="PhotinoAppBuilder"/> instance.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="useDefaults">
        /// <see langword="true"/> to configure default configuration sources, logging, and application settings binding; otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>The <see cref="PhotinoAppBuilder"/>.</returns>
        public static PhotinoAppBuilder CreateBuilder(string[]? args = null, bool useDefaults = true) =>
            new(new() { Args = args }, useDefaults);

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotinoAppBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="PhotinoAppOptions"/> to configure the <see cref="PhotinoAppBuilder"/>.</param>
        /// <returns>The <see cref="PhotinoAppBuilder"/>.</returns>
        public static PhotinoAppBuilder CreateBuilder(PhotinoAppOptions options) =>
            new(options);
    }
}
