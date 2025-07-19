using WebUtils.Utils;

namespace WebUtils.Web
{
    public interface IHttpResponseFacade
    {
        IAccessor<string> Headers { get; }
        IAccessor<ICookie> Cookies { get; }
        int StatusCode { get; }
    }
}