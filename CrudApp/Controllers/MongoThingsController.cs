using CrudApp.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    public class MongoThingsController : Controller
    {
        private readonly IMongoThingsRepository _mongoThingsRepository;
        public MongoThingsController(IMongoThingsRepository mongoThingsRepository)
        {
            _mongoThingsRepository = mongoThingsRepository ?? throw new ArgumentNullException(nameof(mongoThingsRepository));
        }

        public async Task<IActionResult> Index()
        {
            
                var mongoThings = await _mongoThingsRepository.GetAllAsync();
                return View(mongoThings);
            
        }

        public async Task<IActionResult> Details(Guid id)
        {
                var thing = await _mongoThingsRepository.GetByIdAsync(id);
                return View(thing);
        }


    }
}
