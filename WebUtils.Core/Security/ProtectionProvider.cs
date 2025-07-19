using Microsoft.AspNetCore.DataProtection;
using WebUtils.Security;

namespace WebUtils.Core.Security
{
    public class ProtectionProvider(IDataProtectionProvider dataProtectionProvider) : IProtectionProvider
    {
        private readonly IDataProtectionProvider dataProtectionProvider = dataProtectionProvider;

        public IProtection CreateProtection(string purpose)
        {
            var dp = dataProtectionProvider.CreateProtector(purpose);
            return new Protection(dp);
        }
    }

    internal class Protection(IDataProtector dp) : IProtection
    {
        private readonly IDataProtector dp = dp;

        public string Protect(string data)
        {
            return dp.Protect(data);
        }

        public string Unprotect(string protectedData)
        {
            return dp.Unprotect(protectedData);
        }
    }
}
