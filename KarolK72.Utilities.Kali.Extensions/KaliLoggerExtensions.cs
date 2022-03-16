using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Extensions
{
    public static class KaliLoggerExtensions
    {
        public static ILoggingBuilder AddKali(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, KaliLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<KaliLoggerOptions, KaliLoggerProvider>(builder.Services);
            return builder;
        }

        public static ILoggingBuilder AddKali(this ILoggingBuilder builder, Action<KaliLoggerOptions> configure)
        {
            builder.AddKali();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
