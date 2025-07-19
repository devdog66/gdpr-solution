using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebUtils.Frmwk.Web;
using WebUtils.Logging;
using WebUtils.Services;

namespace Web.Frmwk.Controllers
{
    public class CookiePrefsController : Controller
    {
        private readonly ILogContract<CookiePrefsController> logger;
        private readonly IConsentServiceClient consentServiceClient;

        public CookiePrefsController(ILogManager logMgr, IConsentServiceClient consentService)
        {
            this.logger = logMgr.GetLogger<CookiePrefsController>();
            this.consentServiceClient = consentService;
        }

        [HttpGet]
        // GET: CookiePrefs
        public ActionResult Index()
        {
            try
            {
                var model = consentServiceClient.GetConsent(new HttpContextAdapter(HttpContext));
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error retrieving cookie preferences. {0}", ex.Message), ex);
                return new HttpStatusCodeResult(500, "Internal server error while retrieving cookie preferences.");
            }
        }

        [HttpPost]
        //[Route("/cookiePrefs")]
        [ValidateAntiForgeryToken]
        public ActionResult Post(List<string> cookieTypes)
        {
            // Always include "Necessary" as it's required
            var selectedCookies = new HashSet<string>(cookieTypes ?? new List<string>())
            {
                "Necessary"
            };

            var cookiePrefs = new CookiePrefsModel
            {
                Necessary = selectedCookies.Contains("Necessary"),
                Analytics = selectedCookies.Contains("Analytics"),
                Marketing = selectedCookies.Contains("Marketing")
            };

            //Save preferences to cookie or user profile as needed
            var contextFacade = new HttpContextAdapter(HttpContext);
            consentServiceClient.SaveConsent(cookiePrefs, contextFacade);
            return View();
        }
    }
}