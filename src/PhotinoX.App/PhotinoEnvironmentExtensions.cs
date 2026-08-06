namespace PhotinoX.App;

/// <summary>
/// Provides extension methods for working with <see cref="PhotinoEnvironment"/>.
/// </summary>
public static class PhotinoEnvironmentExtensions
{
    extension(PhotinoEnvironment environment)
    {
        /// <summary>
        /// Resolves a startup URL or relative web asset path using the Photino environment.
        /// </summary>
        /// <param name="startUrl">The start URL to resolve.</param>
        /// <returns>The resolved start URL.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the environment or <paramref name="startUrl"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="startUrl"/> is empty or whitespace.
        /// </exception>
        public string ResolveStartUrl(string startUrl)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentException.ThrowIfNullOrWhiteSpace(startUrl);

            if (Uri.TryCreate(startUrl, UriKind.Absolute, out _))
                return startUrl;

            if (Path.IsPathRooted(startUrl))
                return startUrl;

            return Path.Combine(environment.WebRootPath, startUrl);
        }
    }
}
