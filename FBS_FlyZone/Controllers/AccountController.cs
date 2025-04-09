using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.EntityFramework;
using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using BusinessLayer.ValidationRules;
using FluentValidation.Results;

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
        public IActionResult Login(User p)
        {
            if (ModelState.IsValid)
            {

            }
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