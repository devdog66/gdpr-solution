using System.Web.Mvc;
using WebUtils.Frmwk.Web;
using WebUtils.Services;

namespace Web.Frmwk.Controllers
{
    public class ConsentBannerController : Controller
    {
        private readonly IConsentServiceClient _consentService;

        public ConsentBannerController(IConsentServiceClient consentService)
        {
            _consentService = consentService;
        }

        // GET: ConsentBanner
        public ActionResult Index()
        {
            var contextFacade = new HttpContextAdapter(HttpContext);
            var consentResponse = _consentService.GetConsent(contextFacade);
            var showBanner = !consentResponse.HasCookie;

            return new PartialViewResult
            {
                ViewName = "_ConsentBanner",
                ViewData = new ViewDataDictionary
                {
                    { "ShowBanner", showBanner }
                }
            };
        }
    }
}