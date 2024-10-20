using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<(IEnumerable<T> Items, int TotalCount)> FindPageAsync(Expression<Func<T, bool>> predicate, int pageSige, int pageNo, List<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBys = null
, params Expression<Func<T, object>>[] includes);

    }
}
