namespace WebUtils.Web
{
    public interface IHttpContextFacade
    {
        IHttpRequestFacade Request { get; }
        IHttpResponseFacade Response { get; }
    }
}
