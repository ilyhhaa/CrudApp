using CrudApp.Contracts;
using CrudApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    public class TransferDataController : Controller
    {
        private readonly IThingsRepository _thingsRepository;
        private readonly IMongoThingsRepository _mongoThingsRepository;

        public TransferDataController(IThingsRepository thingsRepository,IMongoThingsRepository mongoThingsRepository)
        {
                _thingsRepository = thingsRepository;
              _mongoThingsRepository = mongoThingsRepository;
        }
        
        public async Task<IActionResult> TransferMongoToSQLS(Guid id)
        {
            var mongoThing = await _mongoThingsRepository.GetByIdAsync(id);

            if (mongoThing == null)
            {
                return NotFound();
            }



            var sqlsThing = new Thing
            {
                Title = mongoThing.Title,
                Description = mongoThing.Description,
                Id = Guid.NewGuid()
            };

            await _thingsRepository.AddAsync(sqlsThing);

            await _mongoThingsRepository.DeleteAsync(mongoThing.Id);


            return RedirectToAction("Index","MongoThings");

        }


       
        public async Task<IActionResult> TransferSqlsToMongo(Guid id)
        {
            var sqlsThing = await _thingsRepository.GetByIdAsync(id);

            if(sqlsThing == null)
            {
                return NotFound();
            }

            var mongoThing = new MongoThings
            {
                Title = sqlsThing.Title,
                Description = sqlsThing.Description,
                Id = Guid.NewGuid()
            };

            await _mongoThingsRepository.CreateAsync(mongoThing);

            await _thingsRepository.DeleteAsync(id);

            return RedirectToAction("Index","Things");

        }
        
    }
}
