using WebUtils.Web;

namespace WebUtils.Services
{
    public interface IRequestLoggerClient
    {
        long LogRequest(IHttpRequestFacade httpRequestFacade, string sessionId);
        void LogResponse(IHttpResponseFacade httpContextFacade, long requestId);
    }
}
