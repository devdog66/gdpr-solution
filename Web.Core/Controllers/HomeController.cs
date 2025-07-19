using Microsoft.AspNetCore.Mvc;

namespace Web.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
