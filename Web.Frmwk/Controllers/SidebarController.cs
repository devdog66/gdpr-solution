using System;
using System.Web.Mvc;
using WebUtils.Frmwk.Web;
using WebUtils.Services;

namespace Web.Frmwk.Controllers
{
    public class SidebarController : Controller
    {
        private readonly IConsentServiceClient _consentService;

        public SidebarController(IConsentServiceClient consentService)
        {
            _consentService = consentService ?? throw new ArgumentNullException(nameof(consentService));
        }

        // GET: Sidebar
        public ActionResult Index()
        {
            var contextFacade = new HttpContextAdapter(HttpContext);
            var consentResponse = _consentService.GetConsent(contextFacade);
            var showAds = consentResponse.CookiePrefs.Marketing;

            return new PartialViewResult
            {
                ViewName = "_Sidebar",
                ViewData = new ViewDataDictionary
                {
                    { "ShowAds", showAds }
                }
            };
        }
    }
}