// Orm.DAL/Repositories/Concretes/Repository.cs
using Microsoft.EntityFrameworkCore;
using Orm.Core.Entities.Common;
using Orm.DAL.DataStorage.Contexts;
using Orm.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orm.DAL.Repositories.Concretes;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContex _context;
    public DbSet<TEntity> Table { get; set; }

    public Repository(AppDbContex context) // Constructor doğrudan AppDbContex alacak şekilde değiştirildi
    {
        _context = context;
        Table = _context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await Table.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entityToDelete = await Table.FindAsync(id);
        if (entityToDelete != null)
        {
            Table.Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(TEntity entity) // Parametre doğrudan TEntity entity'si olarak değiştirildi
    {
        var existingEntity = await Table.FindAsync(entity.Id);
        if (existingEntity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {entity.Id} not found for update.");
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        _context.Entry(existingEntity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await Table.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await Table.ToListAsync();
    }
}