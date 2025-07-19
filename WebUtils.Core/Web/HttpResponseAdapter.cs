using Microsoft.AspNetCore.Http;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Core.Web
{
    public class HttpResponseAdapter(HttpResponse httpResponse) : IHttpResponseFacade
    {
        private readonly HttpResponse httpResponse = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));

        public IAccessor<string> Headers
        {
            get 
            {
                return new HeadersAccessor(httpResponse.Headers);
            }
        }

        public int StatusCode
        {
            get 
            {
                return httpResponse.StatusCode;
            }
        }

        public IAccessor<ICookie> Cookies
        {
            get 
            {
                return new ResponseCookiesAccessor(httpResponse.Cookies, Headers);
            }
        }
    }
}
