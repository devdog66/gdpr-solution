using Microsoft.AspNetCore.Mvc;

namespace Web.Core.Middleware
{
    public class ErrorHandler : AbstractMiddleware
    {
        private RequestDelegate next;
        private ILogger<ErrorHandler> logger;

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                var viewResult = new ViewResult()
                {
                    ViewName = "~/Views/Errors/Error.cshtml",
                };

                var executor = GetExecutor(context, viewResult);
                var actionContext = GetActionContext(context);

                await executor.ExecuteAsync(actionContext, viewResult);
            }
        }
    }
}
