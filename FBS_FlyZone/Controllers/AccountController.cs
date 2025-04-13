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
 [AllowAnonymous]
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
       
        public async Task<IActionResult>  Login(User p)
        {

            Context c = new Context();
            var datavalue = c.Users.FirstOrDefault(x => x.Email == p.Email && x.UserPassword == p.UserPassword);

            if (datavalue != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, datavalue.Email),
                    new Claim("UserId", datavalue.UserID.ToString())
                };
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Flight", "Flight");
            }
            else
            {
                return View();
            }


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
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var userEmail = User.Identity.Name;
            Context c = new Context();
            var userValues = c.Users.FirstOrDefault(x => x.Email == userEmail);

            if (userValues == null)
            {
                return RedirectToAction("Login");
            }

            return View(userValues);
        }
        
        // GET: ChangePassword 
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            
            return View(new ChangePasswordViewModel());
        }
        
        // POST: ChangePassword
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            
            var userEmail = User.Identity?.Name ?? string.Empty;
            
            bool result = um.ChangePassword(
                userEmail, 
                model.CurrentPassword ?? string.Empty, 
                model.NewPassword ?? string.Empty);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Profile");
            }
            else
            {
                ModelState.AddModelError("CurrentPassword", "Mevcut şifreniz doğru değil.");
                return View(model);
            }
        }

        // Çıkış işlemi
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Flight", "Flight");
        }
    }
}