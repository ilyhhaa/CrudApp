using CrudApp.Contracts;
using CrudApp.Data;
using CrudApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Repositories
{
    public class SQLSThingsRepository : IThingsRepository
    {
        private readonly CrudAppContext _context;

        public SQLSThingsRepository(CrudAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Thing>> GetAllAsync()
        {
            if (_context == null) throw new ArgumentNullException("Entity set 'ApplicationDbContext.Thing' is null.");

            return await _context.Thing.ToListAsync();
        }

        public async Task<Thing> GetByIdAsync(Guid id)
        {
            if (_context.Thing == null)
            {
                throw new InvalidOperationException("Entity set 'ApplicationDbContext.Thing' is null.");
            };

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID value");
            }

            try
            {
                return await _context.Thing.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (DbUpdateException DbUpdateEx)
            {

                throw new Exception("An error occurred while accessing the database");
            }
            catch (InvalidOperationException InvalidOpEx)
            {
                throw new Exception("An error occurred while accessing the database");
            }

        }
        public async Task AddAsync(Thing thing)
        {
            await _context.Thing.AddAsync(thing);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Thing thing)
        {
            _context.Thing.Update(thing);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var thing = await _context.Thing.FindAsync(id);

            if (thing != null)
            {
                _context.Thing.Remove(thing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Thing.AnyAsync(e => e.Id == id);
        }

    }
}

