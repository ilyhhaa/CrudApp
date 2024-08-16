using CrudApp.Data;
using CrudApp.Models;
using MongoDB.Driver;

namespace CrudApp.Repositories
{
    public class MongoThingsRepository
    {
        private readonly MongoThingsContext _context;

        public MongoThingsRepository(MongoThingsContext context)
        {
           _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<MongoThings>> GetAllAsync()
        {
            return await _context.MongoThings.Find(Builders<MongoThings>.Filter.Empty).ToListAsync();
        }

        public async Task<MongoThings> GetByIdAsync(Guid id)
        {
            return await _context.MongoThings.Find(Builders<MongoThings>.Filter.Eq(m => m.Id, id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MongoThings thing)
        {
            thing.Id = Guid.NewGuid();
            await _context.MongoThings.InsertOneAsync(thing);
        }

        public async Task UpdateAsync(Guid id,MongoThings thing)
        {
            var filter = Builders<MongoThings>.Filter.Eq(m => m.Id, id);

            var update = Builders<MongoThings>.Update
                .Set(m => m.Title, thing.Title)
                .Set(m => m.Description, thing.Description);

            await _context.MongoThings.UpdateOneAsync(filter, update);

        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.MongoThings.DeleteOneAsync(Builders<MongoThings>.Filter.Eq(m => m.Id, id));
        }

    }
}
