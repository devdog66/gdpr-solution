using System;
using System.Collections.Generic;
using WebUtils.Config;

namespace WebUtils.Frmwk.Config
{
    public class ConfigAdapter : IConfigContract
    {
        public T GetValue<T>(string configPath)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (appSettings[configPath] != null)
            {
                var value = appSettings[configPath];
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                throw new KeyNotFoundException($"Configuration key '{configPath}' not found.");
            }
        }
    }
}
