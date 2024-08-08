using CrudApp.Contracts;
using CrudApp.Data;
using CrudApp.Models;
using CrudApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests_SQLSThingsRepository
{
    public class ThingsRepositoryTests
    {
        private readonly SQLSThingsRepository _repository;
        private readonly DbContextOptions<CrudAppContext> _options;

        public ThingsRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<CrudAppContext>()
                .UseInMemoryDatabase(databaseName: "ThingsTestDb")
                .Options;

            var context = new CrudAppContext(_options);
            _repository = new SQLSThingsRepository(context);

            Seed(context);
        }

        private void Seed(CrudAppContext context)
        {
            context.Thing.Add(new Thing { Id = Guid.NewGuid(), Title = "Thing1", Description = "Description1" });
            context.Thing.Add(new Thing { Id = Guid.NewGuid(), Title = "Thing2", Description = "Description2" });
            context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllThings()
        {

            await using var context = new CrudAppContext(_options);
            var repository = new SQLSThingsRepository(context);

            
            var result = await repository.GetAllAsync();

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_ContextIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SQLSThingsRepository(null));
        }
    }
}