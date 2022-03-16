using KarolK72.Utilities.Kali.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddLogging((lg) =>
{
    lg.ClearProviders();
    lg.SetMinimumLevel(LogLevel.Debug);
    lg.AddKali((options) =>
    {
        options.SourceName = "TestingLoggingClient";
        options.Port = 7001;
        options.IPAddress = "https://localhost";
        options.SourceGUID = Guid.Parse("0F08A6D3-3DB5-4D1D-BB87-583BB8545EBD");
    });
});



var serviceProvider = serviceCollection.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Test log");

using(logger.BeginScope("Test Scope"))
{
    using(logger.BeginScope("Another scope"))
    {
        logger.LogWarning("Inside of 2 scopes at {dt}", DateTime.Now.ToString("s"));

    }

    logger.LogWarning("Inside of inner scope");
}

logger.LogWarning("Out!");

logger.LogError(new Exception("Oof"),"Test log!");

serviceProvider.Dispose();