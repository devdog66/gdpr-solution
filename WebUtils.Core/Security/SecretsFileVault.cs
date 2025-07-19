using Microsoft.Extensions.Configuration;
using WebUtils.Security;

namespace WebUtils.Core.Security
{
    public class SecretsFileVault(IConfiguration configuration) : IVault
    {
        private readonly IConfiguration configuration = configuration;

        public string GetSecret(string key)
        {
            return configuration.GetValue<string>(key) 
                   ?? throw new KeyNotFoundException($"Secret with key '{key}' not found in configuration.");
        }
    }
}
