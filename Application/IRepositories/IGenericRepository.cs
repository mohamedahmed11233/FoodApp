using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepositories
{
   public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T item);
        public Task<IQueryable<T>> GetAllWithSpecAsync(Expression<Func<T, bool>> criteria);
        public Task<T> GetBySpecAsync(Expression<Func<T, bool>> criteria);
        public Task UpdateInclude(T entity, params string[] modifiedProperties);

    }

}
