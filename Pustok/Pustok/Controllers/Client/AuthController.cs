using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pustok.Database;
using Pustok.Services.Abstract;
using Pustok.ViewModels.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client
{
    public class AuthController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;
        private readonly IUserService _userService;

        public AuthController(PustokDbContext pustokDbContext)
        {
            _pustokDbContext = pustokDbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _pustokDbContext.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            if(user == null)
            {
                ModelState.AddModelError("Email", "Email or Password is wrong!");
                return View();
            }

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString())
            };

            var claimsIdentity= new ClaimsIdentity(claims, "Cookie");
            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Cookie",claimsPricipal);

            return RedirectToAction("index", "home");
        }
        
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookie");

            return RedirectToAction(nameof(Login));
        }
    }

}
