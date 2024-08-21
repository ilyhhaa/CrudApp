using CrudApp.Contracts;

namespace CrudApp.Models.SeedData
{
    public class SeedData
    {
        private readonly IMongoThingsRepository _mongoThingsRepository;

        public SeedData(IMongoThingsRepository mongoThingsRepository)
        {
            _mongoThingsRepository = mongoThingsRepository;
        }

        public async Task Initialize()
        {
            var existingThings = await _mongoThingsRepository.GetAllAsync();
            if (existingThings.Any())
            {
                return;
            }

            var things = new List<MongoThings>
        {
            new MongoThings { Title = "First Thing", Description = "Description for the first thing" },
            new MongoThings { Title = "Second Thing", Description = "Description for the second thing" },
          
        };

            foreach (var thing in things)
            {
                await _mongoThingsRepository.CreateAsync(thing);
            }
        }
    }
}
