using System.Web;
using WebUtils.Utils;
using WebUtils.Web;

namespace WebUtils.Frmwk.Web
{
    internal class HttpRequestAdapter : IHttpRequestFacade
    {
        private readonly HttpRequestBase httpRequest;

        public HttpRequestAdapter(HttpRequestBase httpRequest)
        {
            this.httpRequest = httpRequest;
        }

        public string UserAgent
        {
            get {                 
                return Headers.Get("User-Agent");
            }
        }

        public IAccessor<string> Headers
        {
            get { 
                return new HeadersAccessor(httpRequest.Headers);
            }
        }

        public string Host
        {             
            get {
                return httpRequest.Url.Host;
            }
        }

        public string Method
        {             
            get {
                return httpRequest.HttpMethod;
            }
        }

        public string Path 
        {
            get {
                return httpRequest.Url.AbsolutePath;
            }
        }

        public string Referer
        {
            get { 
                return httpRequest.UrlReferrer?.AbsoluteUri ?? string.Empty;
            }
        }

        public IAccessor<ICookie> Cookies
        {
            get { 
                return new RequestCookiesAccessor(httpRequest.Cookies);
            }
        }
    }
}
