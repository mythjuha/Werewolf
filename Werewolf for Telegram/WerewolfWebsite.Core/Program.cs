
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BotConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:BotConnectionString is required.");
}

WWCoreContext.Configure(connectionString);
builder.Services.AddSystemWebAdapters();
builder.Services.AddHttpForwarder();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<WWCoreContext>((services, options) =>
{
    var configuredConnectionString = services.GetRequiredService<IConfiguration>()
        .GetConnectionString("BotConnectionString");

    options.UseSqlServer(configuredConnectionString);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapDefaultControllerRoute();
var proxyTo = app.Configuration["ProxyTo"];
if (!string.IsNullOrWhiteSpace(proxyTo))
{
    app.MapForwarder("/{**catch-all}", proxyTo)
        .Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);
}

app.Run();
