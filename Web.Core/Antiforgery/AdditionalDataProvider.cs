
using Microsoft.AspNetCore.Antiforgery;

namespace Web.Core.Antiforgery
{
    public class AdditionalDataProvider : IAntiforgeryAdditionalDataProvider
    {
        public string GetAdditionalData(HttpContext context)
        {
            try
            {
                var session = context.Session;
                if (session == null) return string.Empty;
                var secretId = session.GetString("secretId");
                return secretId ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool ValidateAdditionalData(HttpContext context, string additionalData)
        {
            try
            {
                var session = context.Session;
                if (session == null) return false;
                var secretId = session.GetString("secretId");
                return secretId != null && secretId == additionalData;
            }
            catch (Exception)
            {
                return false; // Log the exception if needed
            }
        }
    }
}