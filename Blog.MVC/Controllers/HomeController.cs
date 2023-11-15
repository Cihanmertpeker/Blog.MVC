using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
