using BlogMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            return View();
        }

    } 

}
