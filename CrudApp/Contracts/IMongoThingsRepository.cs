using CrudApp.Models;

namespace CrudApp.Contracts
{
    public interface IMongoThingsRepository
    {
        Task<IEnumerable<MongoThings>> GetAllAsync();
        Task<MongoThings> GetByIdAsync(Guid id);
        Task CreateAsync(MongoThings thing);
        Task UpdateAsync(Guid id, MongoThings thing);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<MongoThings>> SearchAsync(string searchString);
    }
}
