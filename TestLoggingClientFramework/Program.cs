using KarolK72.Utilities.Kali.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestLoggingClientFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging((lg) =>
            {
                lg.ClearProviders();
                lg.SetMinimumLevel(LogLevel.Debug);
                lg.AddKali((options) =>
                {
                    options.SourceName = "TestingLoggingClient";
                    options.Port = 5000;
                    options.IPAddress = "http://localhost";
                    options.SourceGUID = Guid.Parse("0F08A6D3-3DB5-4D1D-BB87-583BB8545EBD");
                });
            });



            var serviceProvider = serviceCollection.BuildServiceProvider();

            //can cause an exception. need to rethink this
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Test log");

            using (logger.BeginScope("Test Scope"))
            {
                using (logger.BeginScope("Another scope"))
                {
                    Thread.Sleep(1000);
                    logger.LogWarning("Inside of 2 scopes at {dt}", DateTime.Now.ToString("s"));

                }

                logger.LogWarning("Inside of inner scope");
            }


            Thread.Sleep(3000);

            logger.LogWarning("Out!");

            logger.LogError(new Exception("Oof"), "Test log!");

            serviceProvider.Dispose();


        }
    }
}
