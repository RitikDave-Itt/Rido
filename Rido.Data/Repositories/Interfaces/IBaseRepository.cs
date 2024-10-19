using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate);            
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

    }
}
