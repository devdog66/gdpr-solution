using System.Web;
using WebUtils.Web;

namespace WebUtils.Frmwk.Web
{
    public class HttpContextAdapter : IHttpContextFacade
    {
        private readonly HttpContextBase context;

        public HttpContextAdapter(HttpContextBase context)
        {
            this.context = context;
        }

        public IHttpRequestFacade Request
        {
            get
            {
                return new HttpRequestAdapter(context.Request);
            }
        }

        public IHttpResponseFacade Response
        {
            get
            {
                return new HttpResponseAdapter(context.Response);
            }
        }
    }
}
