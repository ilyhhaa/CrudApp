﻿using CrudApp.Models;


namespace CrudApp.Contracts
{
    public interface IThingsRepository
    {
        Task<IEnumerable<Thing>> SearchAsync(string searchString);
        Task<IEnumerable<Thing>> GetAllAsync();
        Task<Thing> GetByIdAsync(Guid id);
        Task AddAsync(Thing thing);
        Task UpdateAsync(Thing thing);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);

    }
}