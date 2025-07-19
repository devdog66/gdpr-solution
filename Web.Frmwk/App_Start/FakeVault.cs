using System.Collections.Generic;
using WebUtils.Security;

namespace Web.Frmwk.App_Start
{
    public class FakeVault : IVault
    {
        public string GetSecret(string key)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (appSettings[key] != null)
            {
                return appSettings[key];
            }
            else
            {
                throw new KeyNotFoundException($"Secret key '{key}' not found.");
            }
        }
    }
}