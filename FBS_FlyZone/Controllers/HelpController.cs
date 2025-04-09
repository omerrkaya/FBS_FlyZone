using Microsoft.AspNetCore.Mvc;

namespace FBS_FlyZone.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult HelpCenter()
        {
            return View();
        }

        public IActionResult Feedback()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Announcements()
        {
            return View();
        }

        public IActionResult FrequentlyAskedQuestions()
        {
            return View();
        }
    }
} 