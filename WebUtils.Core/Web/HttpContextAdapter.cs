using Microsoft.AspNetCore.Http;
using WebUtils.Web;

namespace WebUtils.Core.Web
{
    public class HttpContextAdapter(HttpContext httpContext) : IHttpContextFacade
    {
        private readonly HttpContext httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));

        public IHttpRequestFacade Request 
        {
            get 
            { 
                return new HttpRequestAdapter(httpContext.Request);
            }
        }

        public IHttpResponseFacade Response
        {
            get
            { 
                return new HttpResponseAdapter(httpContext.Response);
            }
        }
    }
}
