// Orm.DAL/Repositories/Interfaces/IRepository.cs
using Orm.Core.Entities.Common;

namespace Orm.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task DeleteAsync(int id);
    Task UpdateAsync(TEntity entity); // Metodun parametresi TEntity olarak değiştirildi
    Task<TEntity> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
}