using Microsoft.AspNetCore.Mvc;

namespace ChapinWarriors.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Error() => View();
    }
}