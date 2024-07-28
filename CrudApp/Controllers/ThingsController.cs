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

                try
                {
                    await _thingsRepository.AddAsync(thing);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentNullException ArgNullEx)
                {
                    ModelState.AddModelError(string.Empty, "The thing cannot be null.");
                }
                catch (ArgumentException ArgEx)
                {
                    ModelState.AddModelError(string.Empty, ArgEx.Message);
                }
                catch (InvalidOperationException InvalidOpEx)
                {
                    ModelState.AddModelError(string.Empty, InvalidOpEx.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
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
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _thingsRepository.UpdateAsync(thing);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentNullException ArgNullEx)
                {
                    
                    ModelState.AddModelError(string.Empty, "The thing cannot be null.");
                }
                catch (InvalidOperationException InvalidOpEx)
                {
                    
                    ModelState.AddModelError(string.Empty, InvalidOpEx.Message);
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }
            return View(thing);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "The ID cannot be null.");
                return View("Error");
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _thingsRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Error");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View("Error");
            }
        }



    }
}
