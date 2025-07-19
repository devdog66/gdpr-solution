
using Microsoft.AspNetCore.Mvc;

namespace Web.Core.Middleware
{
    public class NotFoundHandler : AbstractMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<NotFoundHandler> logger;

        public NotFoundHandler(RequestDelegate next, ILogger<NotFoundHandler> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public override async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
                if (context.Response.StatusCode == 404)
                {
                    var path = context.Request.Path;
                    var logMsg = string.Format("Request for non existent resource {0}", path);
                    logger.LogInformation(logMsg);

                    var viewResult = new ViewResult()
                    {
                        ViewName = "~/Views/Errors/NotFound.cshtml",
                    };

                    var executor = GetExecutor(context, viewResult);
                    var actionContext = GetActionContext(context);

                    await executor.ExecuteAsync(actionContext, viewResult);
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("NotFoundHandler:Invoke threw exception {0}", ex.Message);
                logger.LogError(message, ex);
            }
        }
    }
}
