using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Login işlemlerini burada yapıyoruz
        }
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kayıt işlemlerini burada yapıyoruz
        }
        return View(model);
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }
}