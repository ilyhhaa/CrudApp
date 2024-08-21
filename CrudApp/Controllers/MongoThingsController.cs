using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    public class MongoThingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
