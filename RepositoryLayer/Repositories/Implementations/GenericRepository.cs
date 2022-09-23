using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void  Add(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
        }
        public void Update(TEntity item)
        {
            _context.Set<TEntity>().Update(item);
        }
        public void Delete(TEntity item)
        {
            _context.Set<TEntity>().Remove(item);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public IQueryable<TEntity> GetAllAsIQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }
    }
}
