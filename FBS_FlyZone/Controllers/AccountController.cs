using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;

namespace FBS_FlyZone.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Burada gerçek giriş işlemleri yapılacak
                // Şimdilik kullanıcı adı ve şifre kontrolü yapmadan ana sayfaya yönlendiriyoruz
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Burada gerçek kayıt işlemleri yapılacak
                // Şimdilik kullanıcı kaydı yapmadan giriş sayfasına yönlendiriyoruz
                return RedirectToAction("Login", "Account");
            }
            return View(model);
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