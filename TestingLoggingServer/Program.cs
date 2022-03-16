using KarolK72.Utilities.Kali.Library.Services;
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
        });
        webBuilder.UseContentRoot(AppDomain.CurrentDomain.BaseDirectory);
        //webBuilder.UseUrls(url);
        webBuilder.Configure(appBuilder =>
        {
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<LoggingGRPCServerService>();
            });
        });

    })
    .Build();


await host.RunAsync();
