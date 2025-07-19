using WebUtils.Utils;

namespace WebUtils.Web
{
    public interface IHttpRequestFacade
    {
        string UserAgent { get; }
        IAccessor<string> Headers { get; }
        string Host { get; }
        string Method { get; }
        string Path { get; }
        string Referer { get; }
        IAccessor<ICookie> Cookies { get; }
    }
}
