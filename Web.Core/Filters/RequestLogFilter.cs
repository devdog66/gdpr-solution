using Microsoft.AspNetCore.Mvc.Filters;
using WebUtils.Core.Web;
using WebUtils.Services;

namespace Web.Core.Filters
{
    public class RequestLogFilter(ILogger<RequestLogFilter> logger) : IResourceFilter
    {
        private readonly ILogger<RequestLogFilter> logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //private readonly IRequestLogger requestLogger = requestLogger ?? throw new ArgumentNullException(nameof(requestLogger));

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            try
            {
                logger.LogDebug("OnResourceExecuted called.");
                var httpContext = context.HttpContext;
                var requestId = httpContext.Items["requestId"] as long?;
                if (requestId == null)
                {
                    logger.LogWarning("Request ID is not set in HttpContext items.");
                    return;
                }
                var response = httpContext.Response;
                var facade = new HttpResponseAdapter(response);
                //requestLogger.LogResponse(facade, (long)requestId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "OnResourceExecuted had error. {errmsg}", ex.Message);
            }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            try
            {
                logger.LogDebug("OnResourceExecuting called.");
                var httpContext = context.HttpContext;
                var request = httpContext.Request;
                var session = httpContext.Session;

                if (!request.Cookies.ContainsKey("seid"))
                {
                    session.SetString("secretId", Guid.NewGuid().ToString());
                }
                logger.LogDebug("Session ID: {SessionId}", session.Id);

                var facade = new HttpRequestAdapter(request);
                //var requestId = requestLogger.LogRequest(facade, session.Id);
                //httpContext.Items.Add("requestId", requestId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "OnResourceExecuting had error. {errmsg}", ex.Message);
            }
            
        }
    }
}
