using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Web.Core.Middleware;

namespace Web.Core.Controllers
{
    public class AppController : Controller
    {
        private readonly ILogger logger;
        private readonly HtmlContent htmlContent;

        public AppController(ILogger<AppController> logger, HtmlContent htmlContent)
        {
            this.logger = logger;
            this.htmlContent = htmlContent;
        }


        [HttpGet]
        [Route("/app/{slug}")]
        public IActionResult Index()
        {
            try
            {
                //Get html content
                //var htmlContent = new HtmlContent(HttpContext.);
                var html = htmlContent.Get("/client-app/dist/index.html");
                //Replace ##Client_Config##
                //var configuredHtml = html.Replace("##Client_Config##", encodedConfig);
                return Content(html, new MediaTypeHeaderValue("text/html"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return new StatusCodeResult(500);
            }
        }
    }
}
