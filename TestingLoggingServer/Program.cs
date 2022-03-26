using KarolK72.Data.Common;
using KarolK72.Utilities.Kali.Common;
using KarolK72.Utilities.Kali.Server.Library.Services;
using KarolK72.Utilities.Kali.Server.PostgreSQL;
using KarolK72.Utilities.Kali.Server.SQLite;
using KarolK72.Utilities.Kali.Server.SQLServer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Data;
using System.Net;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddGrpc();
        //services.AddSingleton(
        //    (x) => (ISqlProviderFactory<ISqlProvider>)new PostgreSQLProviderFactory(new PostgreSQLOptions() { ConnectionString = "User ID=postgres;Password=karol123;Host=192.168.1.122;Port=5432;Database=KaliDb;Connection Lifetime=0;" }));
        //services.AddSingleton(
        //    (x) => (ISqlProviderFactory<ISqlProvider>)new SQLiteProviderFactory(new SQLiteOptions() { ConnectionString = @"Data Source=F:\Documents\dbTesting\test.db;Version=3;Pooling=True;Max Pool Size=100" }));
        services.AddSingleton(
            (x) => (ISqlProviderFactory<ISqlProvider>)new SQLServerProviderFactory(new SQLServerOptions() { ConnectionString = @"Data Source=192.168.1.122,1433;Initial Catalog=KaliDb;User ID=SA;Password=SQLServerPassword123!;TrustServerCertificate=True" }));
        services.AddTransient((x) => x.GetRequiredService<ISqlProviderFactory<ISqlProvider>>().CreateNew());
        services.AddSingleton(
            (x) => (IUnitOfWorkFactory<ISqlProvider>)new UnitOfWorkFactory<ISqlProvider>
                (() => x.GetRequiredService<ISqlProvider>(),
                    (sql) => sql.BeginTransaction(),
                    (dis) => ((IDbTransaction)dis).Commit(),
                    (dis) => Task.Run(() => ((IDbTransaction)dis).Commit())
                )
            );
        services.AddSingleton<ILoggingAggregatorService, LoggingAggregatorService>();
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(LogLevel.Debug);
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
