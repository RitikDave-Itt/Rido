using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rido.Data.Entities;
using Rido.Data.Contexts;
using Rido.Data.Repositories.Interfaces;

namespace Rido.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly RidoDbContext _context;

        public UserRepository(RidoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
