using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Imbracaminte.Models;
using System.Security.Claims;
using Imbracaminte.Models.VMs;
using Imbracaminte.Models.Entities;

namespace Imbracaminte.Controllers
{
    public class LoginController : Controller
    {
        private readonly ImbracaminteContext context;

        public LoginController(IWebHostEnvironment hostEnvironment, ImbracaminteContext context)
        {
            this.context = context;

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");
                return View(loginVM);
            }

            var user = context.Users.Select( u =>  u.UserName == loginVM.UserName && u.Password == loginVM.Password ).FirstOrDefault();
           
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                return View(loginVM);
            }


            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, loginVM.UserName),
            };

            ClaimsIdentity claimsIdentity= new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
