﻿using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Pustok.Database;
using Pustok.Database.DomainModels;
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

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _pustokDbContext.Users.
                Include(x=> x.UserRole).
                FirstOrDefault(x => x.Email == model.Email);

            if(user == null)
            {
                ModelState.AddModelError("Email", "Email or Password is wrong!");
                return View();
            }

            if(!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("Email", "Email or Password is wrong!");
                return View();
            }


            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString())
            };

            foreach (var userRole in user.UserRole)
            {

                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.ToString()));
            }



            var claimsIdentity= new ClaimsIdentity(claims, "Cookie");
            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Cookie",claimsPricipal);

            return RedirectToAction("index", "home");
        }


        #endregion

        #region Register

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if(_pustokDbContext.Users.Any(x=>x.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already taken");
                return View();
            }

            var user = new User
            {
                Name = model.Name,
                LastName = model.Surname,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _pustokDbContext.Users.Add(user);
            _pustokDbContext.SaveChanges();

            return RedirectToAction("index","home");
        }




        #endregion



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookie");

            return RedirectToAction(nameof(Login));
        }
    }

}
