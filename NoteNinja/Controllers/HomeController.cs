using Microsoft.AspNetCore.Mvc;

namespace NoteNinja.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Notes");
        }
    }
}
