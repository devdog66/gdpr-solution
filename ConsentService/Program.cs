using WebUtils.Core.Filters;
using WebUtils.Core.Security;
using WebUtils.Data;
using WebUtils.Domain.Repositories;
using WebUtils.Security;

internal class Program
{
    private static int Main(string[] args)
    {
        var logger = GetLogger();
        try
        {
            var webAppOptions = new WebApplicationOptions
            {
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

        app.MapControllers();

        return app;
    }

    internal static WebApplicationBuilder CreateBuilder(WebApplicationOptions webAppOptions, ILogger logger)
    {
        var builder = WebApplication.CreateBuilder(webAppOptions);

        var configCount = SetupConfiguration(builder.Configuration, builder.Environment, logger);
        if (configCount < 1) throw new Exception("No configuration sources found.");

        SetupServices(builder.Services, logger);

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
            .AddJsonFile("secrets.json", true, true)
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true);
        logger.LogInformation("Configuration sources set up successfully.");
        return configuration.Sources.Count;
    }

    internal static void SetupServices(IServiceCollection services, ILogger logger)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IVault, SecretsFileVault>();
        services.AddScoped<IRepository, BaseRepository>();

        services.AddScoped<ApiKeyAuthorizationFilter>();

        //Setup Controllers or Razor
        services.AddControllers();

        logger.LogInformation("Services configured successfully.");
    }

    
}