using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IBaseService<T> where T : class
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
