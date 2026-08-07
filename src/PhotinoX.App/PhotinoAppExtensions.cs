namespace PhotinoX.App;

/// <summary>
/// Provides builder factory methods for <see cref="PhotinoApp"/>.
/// </summary>
public static class PhotinoAppExtensions
{
    extension (PhotinoApp app)
    {
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
        /// Creates a new <see cref="PhotinoAppBuilder"/> instance using the specified options.
        /// </summary>
        /// <param name="options">The <see cref="PhotinoAppOptions"/> used to configure the <see cref="PhotinoAppBuilder"/>.</param>
        /// <param name="useDefaults">
        /// <see langword="true"/> to configure default configuration sources, logging, and application settings binding; otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>The <see cref="PhotinoAppBuilder"/>.</returns>
        public static PhotinoAppBuilder CreateBuilder(PhotinoAppOptions options, bool useDefaults = true) =>
            new(options, useDefaults);
    }
}
