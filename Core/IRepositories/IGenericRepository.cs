using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepositories
{
   public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T item);
        public Task<IQueryable<T>> GetAllWithSpecAsync(Expression<Func<T, bool>> criteria);
        public  Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null!, params Expression<Func<T, object>>[] includeProperties);

        public Task<T> GetBySpecAsync(Expression<Func<T, bool>> criteria);
        public Task UpdateInclude(T entity, params string[] modifiedProperties);
        public Task<int> SaveChangesAsync();
        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }

}
