using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rido.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
