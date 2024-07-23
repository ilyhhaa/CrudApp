using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    public class FunController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Welcome(string name = "Ilya", string surname = "Kunitski", int times = 5)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["Surname"] = surname;
            ViewData["Times"] = times;

            return View();
        }
    }
}
