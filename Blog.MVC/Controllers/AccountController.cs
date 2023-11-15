using BlogMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogMVC.Data;
using BlogMVC.Validators;

namespace BlogMVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly BlogDbContext context;
        public AccountController(BlogDbContext context)
        {
            this.context = context;
        }

        public IActionResult Login()
        {
            
            return View(new UserLoginModel());
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel model)
        {
            var validator = new UserLoginModelValidator();
            var validationResult = validator.Validate(model);

            if (validationResult.IsValid) 
            {

                var appUser = this.context.AppUsers.SingleOrDefault(x => x.Username == model.UserName && x.Password == model.Password);
                if (appUser != null)
                {
                 await  SetAuthCookiee(appUser, model.RememberMe);
                    return RedirectToAction("Index", "Category", new {area="Admin"});
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    return View();
                }
              
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName,error.ErrorMessage);
                }
                return View(model);
            }
                      

          
        }
        private async Task SetAuthCookiee(AppUser user,bool rememberMe)
        {
            var claims = new List<Claim>
        {
           
            new Claim("FirtName", user.Name),
            new Claim("Surname", user.Surname),
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = rememberMe
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }
    }

} 
