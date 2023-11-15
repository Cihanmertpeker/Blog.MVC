using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Controllers.Admin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
