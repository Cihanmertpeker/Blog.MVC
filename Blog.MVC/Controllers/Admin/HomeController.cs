using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers.Admin
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
