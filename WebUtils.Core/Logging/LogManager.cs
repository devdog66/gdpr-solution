using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebUtils.Logging;

namespace WebUtils.Core.Logging
{
    public class LogManager(IServiceProvider serviceProvider) : ILogManager
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public ILogContract<T> GetLogger<T>()
        {
            var logger = serviceProvider.GetRequiredService<ILogger<T>>();
            return new DotnetCoreLogger<T>(logger);
        }
    }

    internal class DotnetCoreLogger<T>(ILogger<T> logger) : ILogContract<T>
    {
        private readonly ILogger<T> logger = logger;

        public void Debug(string message)
        {
            logger.LogDebug("{Message}", message); 
        }

        public void Error(string message)
        {
            logger.LogError("{Message}", message); 
        }

        public void Error(string message, Exception exception)
        {
            logger.LogError(exception, "{Message}", message); 
        }

        public void Fatal(string message)
        {
            logger.LogCritical("{Message}", message);
        }

        public void Fatal(string message, Exception exception)
        {
            logger.LogCritical(exception, "{Message}", message); 
        }

        public void Info(string message)
        {
            logger.LogInformation("{Message}", message); 
        }

        public void Warn(string message)
        {
            logger.LogWarning("{Message}", message); 
        }

        public void Warn(string message, Exception exception)
        {
            logger.LogWarning(exception, "{Message}", message); 
        }
    }
}
