using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;

namespace Rido.Services
{
    public class BaseService<T>  :IBaseService<T>  where T : class 
    {
        protected readonly IBaseRepository<T> _repository;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService(IBaseRepository<T> repository , IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = contextAccessor;
        }

        protected string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }

        public async Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindFirstAsync(predicate);
        }

     
        
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindAllAsync(predicate);
        }
    }
}
