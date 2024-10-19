using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rido.Data.Contexts;
using Rido.Data.Repositories.Interfaces;

namespace Rido.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly RidoDbContext _context;

        public BaseRepository(RidoDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;     
        }


        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstAsync(predicate);
        }

     

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
