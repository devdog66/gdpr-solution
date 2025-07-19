using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Web.Core.Middleware
{
    public abstract class AbstractMiddleware
    {
        public abstract Task Invoke(HttpContext context);

        protected IActionResultExecutor<ViewResult> GetExecutor(HttpContext context, ViewResult viewResult)
        {
            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                                new ModelStateDictionary());
            viewDataDictionary.Model = //your model
            viewResult.ViewData = viewDataDictionary;


            var executor = context.RequestServices
                .GetRequiredService<IActionResultExecutor<ViewResult>>();
            return executor;
        }

        protected ActionContext GetActionContext(HttpContext context)
        {
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData,
                new ActionDescriptor());
            return actionContext;
        }
    }

}
