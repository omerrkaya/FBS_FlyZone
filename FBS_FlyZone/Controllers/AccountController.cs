using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.EntityFramework;
using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using BusinessLayer.ValidationRules;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Concrete;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace FBS_FlyZone.Controllers
{
   
    public class AccountController : Controller
    {

        UserManager um = new UserManager(new EfUserRepository());


        // GET: Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // POST: Login
        [HttpPost] 
        //[AllowAnonymous]
        public /*async Task<>*/IActionResult Login(User p)
        {

            //Context c = new Context();
            //var datavalue = c.Users.FirstOrDefault(x => x.Email == p.Email && x.UserPassword == p.UserPassword);

            //if (datavalue != null)
            //{
            //    var claims = new List<Claim>
            //    {
            //        new Claim(ClaimTypes.Name, p.Email)
            //    };
            //    var userIdentity = new ClaimsIdentity(claims, "Login");
            //    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);  
            //    await HttpContext.SignInAsync(principal);
            //    return RedirectToAction("Flight", "Flight");
            //}
            //else
            //{

            //}
            return View();

        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(User p)
        {
            UserValidator userRules = new UserValidator();
            ValidationResult result = userRules.Validate(p);
            if (result.IsValid)
            {
                p.UserRole = "User";
                p.RegisterationDate = DateTime.Now;
                um.RegisterUserAdd(p);

                return RedirectToAction("Flight", "Flight");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();

        }

        // Şifre sıfırlama sayfası
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Profil sayfası
        public IActionResult Profile()
        {
            return View();
        }
    }
}
























//var claims = new List<Claim>
//{
//    new Claim(ClaimTypes.Name, p.Email)
//};
//var userIdentity = new ClaimsIdentity(claims, "Login");
//ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
//await HttpContext.SignInAsync(principal);