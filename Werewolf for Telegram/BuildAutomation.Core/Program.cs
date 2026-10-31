using Yarp.ReverseProxy.Forwarder;
using BuildAutomation.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITelegramNotificationService, TelegramNotificationService>();
builder.Services.AddSystemWebAdapters();
builder.Services.AddHttpForwarder();

var app = builder.Build();

app.UseSystemWebAdapters();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

var proxyTo = builder.Configuration["ProxyTo"];
if (!string.IsNullOrWhiteSpace(proxyTo) && Uri.TryCreate(proxyTo, UriKind.Absolute, out var proxyUri))
{
    app.MapForwarder("/{**catch-all}", proxyUri.ToString());
}

app.Run();
