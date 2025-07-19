using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace WebUtils.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }

    public class ApiKeyAuthorizationFilter(IConfiguration configuration) : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration = configuration;
        private const string ApiKeyHeaderName = "X-API-KEY"; // Or "Authorization" header if you prefer

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Get the API key from the request header
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get the API key from configuration (e.g., appsettings.json)
            var apiKeyFromConfig = _configuration.GetValue<string>("ApiKey");

            // Validate the API key
            if (!apiKeyFromConfig!.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
