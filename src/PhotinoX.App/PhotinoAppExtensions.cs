using Microsoft.Extensions.DependencyInjection;

namespace PhotinoX.App;

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
        /// Initializes a new instance of the <see cref="PhotinoAppBuilder"/> class with optional defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
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
