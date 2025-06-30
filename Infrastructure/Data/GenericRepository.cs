using System;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext storeContext) : IGenericRepository<T> where T : Core.Entities.BaseEntity
{
    public void Create(T entity)
    {
        storeContext.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        storeContext.Set<T>().Remove(entity);
    }

    public bool Exists(int id)
    {
        return storeContext.Set<T>().Any(e => e.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await storeContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await storeContext.Set<T>().FindAsync(id).AsTask();
    }

    public async Task<bool> SaveChangesAsync()
    {
       return await storeContext.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        storeContext.Set<T>().Attach(entity);
        storeContext.Entry(entity).State = EntityState.Modified;
    }
}
