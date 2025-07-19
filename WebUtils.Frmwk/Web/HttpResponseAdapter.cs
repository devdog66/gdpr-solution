using System.Web;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Frmwk.Web
{
    public class HttpResponseAdapter : IHttpResponseFacade
    {
        private readonly HttpResponseBase _response;

        public HttpResponseAdapter(HttpResponseBase response)
        {
            _response = response;
        }

        public IAccessor<string> Headers
        {
            get 
            {
                return new HeadersAccessor(_response.Headers);
            }
        }

        public IAccessor<ICookie> Cookies
        {
            get
            {
                return new ResponseCookiesAccessor(_response.Cookies);
            }
        }
        

        public int StatusCode
        { 
            get 
            { 
                return _response.StatusCode; 
            }
        }
    }
}
