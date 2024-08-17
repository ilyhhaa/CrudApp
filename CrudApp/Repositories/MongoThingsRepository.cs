using CrudApp.Contracts;
using CrudApp.Data;
using CrudApp.Models;
using MongoDB.Driver;

namespace CrudApp.Repositories
{
    public class MongoThingsRepository : IMongoThingsRepository
    {
        private readonly MongoThingsContext _context;

        public MongoThingsRepository(MongoThingsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<MongoThings>> GetAllAsync()
        {
            try
            {
                return await _context.MongoThings.Find(Builders<MongoThings>.Filter.Empty).ToListAsync();
            }
            catch (MongoException mongoEx)
            {
                throw new InvalidOperationException("An error occurred while retrieving the items.", mongoEx);
            }
        }

        public async Task<MongoThings> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "The ID cannot be empty.");
            }

            try
            {
                var thing = await _context.MongoThings.Find(Builders<MongoThings>.Filter.Eq(m => m.Id, id)).FirstOrDefaultAsync();

                if (thing == null)
                {
                    throw new InvalidOperationException($"No item found with ID {id}.");
                }

                return thing;
            }
            catch (MongoException mongoEx)
            {
                throw new InvalidOperationException("An error occurred while retrieving the item.", mongoEx);
            }
        }

        public async Task CreateAsync(MongoThings thing)
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing), "The item to create cannot be null.");
            }

            try
            {
                thing.Id = Guid.NewGuid();
                await _context.MongoThings.InsertOneAsync(thing);
            }
            catch (MongoException mongoEx)
            {
                throw new InvalidOperationException("An error occurred while creating the item.", mongoEx);
            }
        }

        public async Task UpdateAsync(Guid id, MongoThings thing)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "The ID cannot be empty.");
            }

            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing), "The item to update cannot be null.");
            }

            try
            {
                var filter = Builders<MongoThings>.Filter.Eq(m => m.Id, id);

                var update = Builders<MongoThings>.Update
                    .Set(m => m.Title, thing.Title)
                    .Set(m => m.Description, thing.Description);

                var updateResult = await _context.MongoThings.UpdateOneAsync(filter, update);

                if (updateResult.MatchedCount == 0)
                {
                    throw new InvalidOperationException($"No item found with ID {id} to update.");
                }
            }
            catch (MongoException mongoEx)
            {
                throw new InvalidOperationException("An error occurred while updating the item.", mongoEx);
            }

        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "The ID cannot be empty.");
            }

            var filter = Builders<MongoThings>.Filter.Eq(m => m.Id, id);

            try
            {
                var deleteResult = await _context.MongoThings.DeleteOneAsync(filter);

                if (deleteResult.DeletedCount == 0)
                {
                    throw new InvalidOperationException($"No item found with ID {id}.");
                }
            }
            catch (MongoException mongoEx)
            {
                throw new InvalidOperationException("An error occurred while attempting to delete the item.", mongoEx);
            }
        }

    }
}
