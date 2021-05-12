using leta.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace leta.Data.Repository
{
    public interface IBaseRepository<TEntity, TPrimaryKey> : IDisposable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        TEntity GetById(TPrimaryKey id);
        IEnumerable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(TPrimaryKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();

    }
}
