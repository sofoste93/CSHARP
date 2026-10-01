using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Services;

const string Version = "2.0.0";
var diagnostics = args.Contains("--diagnostics", StringComparer.OrdinalIgnoreCase);
var noBrowser = diagnostics || args.Contains("--no-browser", StringComparer.OrdinalIgnoreCase) || Environment.GetEnvironmentVariable("CI") == "true";
var diagnosticDirectory = diagnostics ? Path.Combine(Path.GetTempPath(), $"violet-pulsar-{Guid.NewGuid():N}") : null;
var dataDirectoryIndex = Array.FindIndex(args, argument => argument.Equals("--data-dir", StringComparison.OrdinalIgnoreCase));
var configuredDataDirectory = dataDirectoryIndex >= 0 && dataDirectoryIndex + 1 < args.Length
    ? Path.GetFullPath(args[dataDirectoryIndex + 1])
    : AppPaths.DataDirectory();
var dataDirectory = diagnosticDirectory ?? configuredDataDirectory;
Directory.CreateDirectory(dataDirectory);
var databasePath = Path.Combine(dataDirectory, "violet-pulsar.db");
if (!diagnostics && !File.Exists(databasePath))
{
    var legacyDatabase = new[] { "TaskManagerApp.db", "app.db" }
        .SelectMany(name => new[] { Path.Combine(Environment.CurrentDirectory, name), Path.Combine(AppContext.BaseDirectory, name) })
        .FirstOrDefault(File.Exists);
    if (legacyDatabase is not null) File.Copy(legacyDatabase, databasePath);
}

var executableDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory;
var publishedWebRoot = Path.Combine(executableDirectory, "wwwroot");
var hasPublishedWebRoot = Directory.Exists(publishedWebRoot);
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = hasPublishedWebRoot ? executableDirectory : Directory.GetCurrentDirectory(),
    WebRootPath = hasPublishedWebRoot ? "wwwroot" : null
});
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")) && !args.Contains("--urls"))
    builder.WebHost.UseUrls("http://127.0.0.1:5274");

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(dataDirectory, "keys")))
    .SetApplicationName("VioletPulsar");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath};Cache=Shared"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<AppText>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[] { new CultureInfo("en"), new CultureInfo("de") };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
    options.RequestCultureProviders = [new CookieRequestCultureProvider()];
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await database.Database.MigrateAsync();
    if (diagnostics)
    {
        _ = await database.Database.CanConnectAsync();
        Console.WriteLine($"Violet Pulsar {Version} · systems nominal");
    }
}

if (diagnostics)
{
    try { Directory.Delete(dataDirectory, true); } catch { }
    return;
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

if (!noBrowser)
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        try { Process.Start(new ProcessStartInfo("http://127.0.0.1:5274") { UseShellExecute = true }); }
        catch { }
    });
}

await app.RunAsync();

public partial class Program { }
