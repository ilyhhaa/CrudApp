using CrudApp.Contracts;
using CrudApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    [Authorize]
    public class MongoThingsController : Controller
    {
        private readonly IMongoThingsRepository _mongoThingsRepository;
        public MongoThingsController(IMongoThingsRepository mongoThingsRepository)
        {
            _mongoThingsRepository = mongoThingsRepository ?? throw new ArgumentNullException(nameof(mongoThingsRepository));
        }

        public async Task<IActionResult> Index(string searchstring)
        {
            var mongoThings = string.IsNullOrEmpty(searchstring)
                ? await _mongoThingsRepository.GetAllAsync()
                : await _mongoThingsRepository.SearchAsync(searchstring);


            return View(mongoThings);

        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var thing = await _mongoThingsRepository.GetByIdAsync(id);
            return View(thing);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] MongoThings thing)
        {
            if (ModelState.IsValid)
            {
                await _mongoThingsRepository.CreateAsync(thing);
                return RedirectToAction(nameof(Index));
            }
            return View(thing);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var thing = await _mongoThingsRepository.GetByIdAsync(id);
            return View(thing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description")] MongoThings thing)
        {
            if (id != thing.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _mongoThingsRepository.UpdateAsync(id, thing);
                return RedirectToAction(nameof(Index));
            }
            return View(thing);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var thing = await _mongoThingsRepository.GetByIdAsync(id);
            return View(thing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _mongoThingsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
