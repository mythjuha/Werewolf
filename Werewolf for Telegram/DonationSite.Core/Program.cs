
using Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BotConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:BotConnectionString is required.");
}

WWCoreContext.Configure(connectionString);
builder.Services.AddSystemWebAdapters();
builder.Services.AddHttpForwarder();
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
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"]!).Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

app.Run();
