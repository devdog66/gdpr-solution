using System;
using System.Security.Cryptography;
using System.Text;
using WebUtils.Security;

namespace WebUtils.Frmwk.Security
{
    public class ProtectionProvider : IProtectionProvider
    {
        public IProtection CreateProtection(string purpose)
        {
            return new Protection(purpose);
        }
    }

    internal class Protection : IProtection
    {
        private readonly byte[] entropy;

        public Protection(string purpose)
        {
            entropy = Encoding.UTF8.GetBytes(purpose);
        }

        public string Protect(string data)
        {
            var protectedBytes = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(data),
                entropy,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(protectedBytes);
        }

        public string Unprotect(string protectedData)
        {
            var unprotectedBytes = ProtectedData.Unprotect(
                Convert.FromBase64String(protectedData),
                entropy,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(unprotectedBytes);
        }
    }
}
