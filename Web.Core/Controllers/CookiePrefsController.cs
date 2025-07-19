using Microsoft.AspNetCore.Mvc;
using WebUtils.Core.Web;
using WebUtils.Services;

namespace Web.Core.Controllers
{
    public class CookiePrefsController(ILogger<CookiePrefsController> logger, IConsentServiceClient consentServiceClient) : Controller
    {
        private readonly ILogger<CookiePrefsController> logger = logger;
        private readonly IConsentServiceClient consentServiceClient = consentServiceClient;

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var model = consentServiceClient.GetConsent(new HttpContextAdapter(HttpContext));
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving cookie preferences. {errmsg}", ex.Message);
                return StatusCode(500, "Internal server error while retrieving cookie preferences.");
            }
        }

        [HttpPost]
        [Route("/cookiePrefs")]
        [ValidateAntiForgeryToken]
        public IActionResult Post(List<string> cookieTypes)
        {
            // Always include "Necessary" as it's required
            var selectedCookies = new HashSet<string>(cookieTypes ?? [])
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
