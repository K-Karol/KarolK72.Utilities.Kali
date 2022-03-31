using KarolK72.Data.Common;
using KarolK72.Utilities.Kali.Common;
using KarolK72.Utilities.Kali.Server.Library.Services;
using KarolK72.Utilities.Kali.Server.SQLServer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Data;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

ISqlProviderFactory<ISqlProvider> sqlProviderFactory = new SQLServerProviderFactory(new SQLServerOptions() { ConnectionString = @"Data Source=192.168.1.122,1433;Initial Catalog=KaliDb;User ID=SA;Password=SQLServerPassword123!;TrustServerCertificate=True" });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddGrpc();
builder.Services.AddSingleton(sqlProviderFactory);
builder.Services.AddTransient((x) => x.GetRequiredService<ISqlProviderFactory<ISqlProvider>>().CreateNew());
builder.Services.AddSingleton(
    (x) => (IUnitOfWorkFactory<ISqlProvider>)new UnitOfWorkFactory<ISqlProvider>
        (() => x.GetRequiredService<ISqlProvider>(),
            (sql) => sql.BeginTransaction(),
            (dis) => ((IDbTransaction)dis).Commit(),
            (dis) => Task.Run(() => ((IDbTransaction)dis).Commit())
        )
    );
builder.Services.AddSingleton<ILoggingAggregatorService, LoggingAggregatorService>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});

builder.WebHost.UseKestrel(webBuilder =>
{
    webBuilder.Listen(IPAddress.Any, 7001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps(".\\dev_cert.pfx",
            "karol");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<KaliServerService>();
});

app.Run();
