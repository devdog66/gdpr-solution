using WebUtils.Web;

namespace WebUtils.Services
{
    public interface IConsentServiceClient
    {
        ConsentResponse GetConsent(IHttpContextFacade contextFacade);
        void SaveConsent(CookiePrefsModel model, IHttpContextFacade contextFacade);
    }

    public class ConsentResponse
    {
        public bool HasCookie { get; set; } = false;
        public CookiePrefsModel CookiePrefs { get; set; } = new CookiePrefsModel();
    }

    public class CookiePrefsModel
    {
        public bool Necessary { get; set; } = true;
        public bool Analytics { get; set; } = false;
        public bool Marketing { get; set; } = false;
    }
}
