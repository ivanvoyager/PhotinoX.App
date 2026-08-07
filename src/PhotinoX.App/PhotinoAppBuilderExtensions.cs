using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PhotinoX.App;

/// <summary>
/// Provides extension methods for configuring <see cref="PhotinoAppBuilder"/> instances.
/// </summary>
public static class PhotinoAppBuilderExtensions
{
    extension(PhotinoAppBuilder builder)
    {
        /// <summary>
        /// Adds application services to the service collection.
        /// </summary>
        /// <param name="configureServices">A delegate used to configure application services.</param>
        /// <returns>The current <see cref="PhotinoAppBuilder"/>.</returns>
        public PhotinoAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configureServices);
            configureServices(builder.Services);
            return builder;
        }

        /// <summary>
        /// Configures common default services, configuration sources, and logging providers.
        /// </summary>
        /// <param name="appOptions">The application options used to configure defaults.</param>
        /// <returns>The current <see cref="PhotinoAppBuilder"/>.</returns>
        internal PhotinoAppBuilder UseDefaults(PhotinoAppOptions appOptions)
        {
            ArgumentNullException.ThrowIfNull(appOptions);

            var contentRootPath = PathResolver.ResolveContentRootPath(appOptions.ContentRootPath, AppContext.BaseDirectory);
            builder.Configuration.SetBasePath(contentRootPath);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            string environmentName = appOptions.GetEnvironmentName(builder.Configuration["PhotinoX:EnvironmentName"] ?? "Production");
            builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

            builder.Configuration.AddEnvironmentVariables();
            if (appOptions.Args is { Length: > 0 })
            {
                builder.Configuration.AddCommandLine(appOptions.Args);
            }

            builder.Services.Configure<PhotinoAppSettings>(builder.Configuration.GetSection("PhotinoX"));

            builder.Services.AddLogging(logging =>
            {
                logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
                logging.AddSimpleConsole();
            });

            return builder;
        }
    }
}
