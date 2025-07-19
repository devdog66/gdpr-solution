using Microsoft.AspNetCore.Antiforgery;
using Web.Core.Antiforgery;
using Web.Core.Filters;
using Web.Core.Middleware;
using WebUtils.Config;
using WebUtils.Core.Config;
using WebUtils.Core.Logging;
using WebUtils.Core.Security;
using WebUtils.Logging;
using WebUtils.Security;
using WebUtils.Services;
using WebUtils.Standard.ServiceClients;
using WebUtils.Standard.Web;
using WebUtils.Web;

internal class Program
{
    private static int Main(string[] args)
    {
        var logger = GetLogger();
        try
        {
            var webAppOptions = new WebApplicationOptions
            {
                WebRootPath = "client-app/dist",
                Args = args
            };
            var app = CreateApp(webAppOptions, logger);
            app.Run();
            return 0;
        }
        catch (Exception ex)
        {

            logger.LogError("Program failed {errMsg}", ex.Message);
            return 1;
        }
    }

    internal static WebApplication CreateApp(WebApplicationOptions webAppOptions, ILogger logger)
    {
        var builder = CreateBuilder(webAppOptions, logger);
        var app = builder.Build();

        app.UseMiddleware<NotFoundHandler>();
        app.UseMiddleware<ErrorHandler>();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        //app.UseHttpsRedirection();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        logger.LogDebug("Web application created.");
        return app;
    }

    internal static WebApplicationBuilder CreateBuilder(WebApplicationOptions webAppOptions, ILogger logger)
    {
        var builder = WebApplication.CreateBuilder(webAppOptions);
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.AddServerHeader = false; // Disable the server header for security reasons
            // Configure Kestrel server options here if needed
            // e.g., serverOptions.Limits.MaxRequestBodySize = 10 * 1024; // 10 KB
        });

        var configCount = SetupConfiguration(builder.Configuration, builder.Environment, logger);
        if (configCount < 1) throw new Exception("No configuration sources found.");

        SetupServices(builder.Services, logger);

        logger.LogInformation("Web application builder created.");
        return builder;
    }

    internal static ILogger GetLogger()
    {
        using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());

        ILogger logger = loggerFactory.CreateLogger<Program>();
        return logger;
    }

    internal static int SetupConfiguration(IConfigurationBuilder configuration, IWebHostEnvironment environment, ILogger logger)
    {
        configuration.SetBasePath(environment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("secrets.json", true, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true);
        logger.LogInformation("Configuration sources loaded: {Count}", configuration.Sources.Count);
        return configuration.Sources.Count;
    }

    internal static void SetupServices(IServiceCollection services, ILogger logger)
    {
        //Setup Dependencies
        services.AddScoped<HtmlContent>();
        //services.AddScoped<IRequestLogger, WebUtils.Standard.RequestLogger>();
        services.AddHttpClient();
        services.AddScoped<ILogManager, LogManager>();
        services.AddScoped<IConfigContract, ConfigAdapter>();
        services.AddScoped<IVault, SecretsFileVault>(); // Replace with actual vault implementation if needed
        services.AddScoped<IConsentServiceClient, ConsentServiceClient>();
        services.AddScoped<IProtectionProvider, ProtectionProvider>();
        services.AddScoped<ICacheFacade, CacheImpl>();

        //Setup Sessions
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
            //options.Cookie.IsEssential = true; // Make the session cookie essential
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use secure cookies
            options.Cookie.SameSite = SameSiteMode.Strict; // Set SameSite policy
            options.Cookie.Domain = "localhost";
            options.Cookie.Path = "/";
            options.Cookie.Name = "seid";
        });

        //Setup Antiforgery
        services.AddAntiforgery(options =>
        {
            // Set Cookie properties using CookieBuilder properties.
            options.Cookie = new CookieBuilder
            {
                Domain = "localhost",//configuration.GetValue<string>("AntiForgery:CookieDomain"),
                Name = "afid",
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                SecurePolicy = CookieSecurePolicy.Always // Use secure cookies
            };
            options.FormFieldName = "aftoken";
            options.HeaderName = "RequestVerificationToken";
            options.SuppressXFrameOptionsHeader = false;
        });
        services.AddSingleton<IAntiforgeryAdditionalDataProvider, AdditionalDataProvider>();

        //Setup Controllers or Razor
        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<RequestLogFilter>();
        });

        logger.LogInformation("Services configured successfully.");
    }
}