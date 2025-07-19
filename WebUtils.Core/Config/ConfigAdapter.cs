using Microsoft.Extensions.Configuration;
using WebUtils.Config;

namespace WebUtils.Core.Config
{
    public class ConfigAdapter(IConfiguration configuration) : IConfigContract
    {
        private readonly IConfiguration? configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public T GetValue<T>(string configPath)
        {
            return configuration!.GetValue<T>(configPath) 
                   ?? throw new InvalidOperationException($"Configuration value for '{configPath}' not found or is null.");
        }
    }
}
