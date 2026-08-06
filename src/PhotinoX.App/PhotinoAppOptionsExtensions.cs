using System.Reflection;

namespace PhotinoX.App;

internal static class PhotinoAppOptionsExtensions
{
    extension(PhotinoAppOptions options)
    {
        internal string GetEnvironmentName(string defaultEnvironmentName = "Production")
        {
            string environmentName = !string.IsNullOrWhiteSpace(options.EnvironmentName)
                ? options.EnvironmentName
                : Environment.GetEnvironmentVariable("PHOTINO_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? (!string.IsNullOrWhiteSpace(defaultEnvironmentName) ? defaultEnvironmentName : "Production");

            return environmentName;
        }

        internal string GetApplicationName(string defaultApplicationName = "PhotinoX")
        {
            string applicationName = !string.IsNullOrWhiteSpace(options.ApplicationName)
                ? options.ApplicationName
                : Assembly.GetEntryAssembly()?.GetName().Name ?? defaultApplicationName;
            return applicationName;
        }
    }
}
