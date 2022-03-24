using KarolK72.Utilities.Kali.Common;
using KarolK72.Utilities.Kali.Server.Library.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddGrpc();
        services.AddSingleton<ILoggingAggregatorService, LoggingAggregatorService>();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddFile(".\\logs.log", append: true);
        });

    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 7001, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
                listenOptions.UseHttps(".\\dev_cert.pfx",
                    "karol");
            });
            options.Listen(IPAddress.Any, 5000, listenOptions =>
             {
                 listenOptions.Protocols = HttpProtocols.Http1;
             });
        });
        webBuilder.UseContentRoot(AppDomain.CurrentDomain.BaseDirectory);
        //webBuilder.UseUrls(url);
        webBuilder.Configure(appBuilder =>
        {
            
            appBuilder.UseRouting();
            appBuilder.UseGrpcWeb();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<KaliServerService>().EnableGrpcWeb();
            });
        });

    })
    .Build();


await host.RunAsync();
