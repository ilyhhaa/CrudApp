using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudApp.Models;
using CrudApp.Contracts;

namespace CrudApp.Controllers
{
    public class ThingsController : Controller
    {
        private readonly IThingsRepository _thingsRepository;

        public ThingsController(IThingsRepository thingsRepository)
        {
            _thingsRepository = thingsRepository;
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _thingsRepository.GetAllAsync());
            }
            catch (InvalidOperationException InvalidOpEx)
            {
                return Problem(detail: InvalidOpEx.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: "An unexpected error occurred. Please try again later.");
            }

        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var thing = await _thingsRepository.GetByIdAsync(id.Value);

                if (thing == null)
                {
                    return NotFound();
                }
                return View(thing);
            }
            catch (ArgumentException ArgumentEx)
            {
                return BadRequest(ArgumentEx.Message);
            }
            catch (InvalidOperationException InvalidOpEx)
            {
                return Problem(detail: InvalidOpEx.Message);
            }
            catch (Exception ex)
            {
                return Problem(detail: "An unexpected error occurred. Please try again later.");
            }

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Thing thing)
        {
            if (ModelState.IsValid)
            {
                thing.Id = Guid.NewGuid();
                await _thingsRepository.AddAsync(thing);
                return RedirectToAction(nameof(Index));
            }
            return View(thing);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _thingsRepository.GetByIdAsync(id.Value);
            if (thing == null)
            {
                return NotFound();
            }
            return View(thing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description")] Thing thing)
        {
            if (id != thing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _thingsRepository.UpdateAsync(thing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _thingsRepository.ExistsAsync(thing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(thing);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thing = await _thingsRepository.GetByIdAsync(id.Value);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _thingsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }



    }
}
