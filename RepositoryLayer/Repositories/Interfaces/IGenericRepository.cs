using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);
        void Delete(TEntity item);
        void Update(TEntity item);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllAsIQueryable();
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);
    }
}
