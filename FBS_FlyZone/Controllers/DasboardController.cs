using Microsoft.AspNetCore.Mvc;
using FBS_FlyZone.Models;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult BookingHistory()
    {
        return View();
    }

    public IActionResult Reviews()
    {
        return View();
    }

    public IActionResult Settings()
    {
        return View();
    }
}