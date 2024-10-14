using AutoMapper;
using Microsoft.AspNetCore.Http;
using Rido.Common.Models.Requests;
using Rido.Data.Entities;
using Rido.Data.Enums;
using Rido.Data.Repositories;
using Rido.Data.Repositories.Interfaces;
using Rido.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rido.Services
{
    public class DriverService : IDriverService
    {
        private readonly IRepository<DriverData> _driverRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DriverService(IRepository<DriverData> driverRepository, IMapper mapper, IRepository<User> userRepository,IHttpContextAccessor httpContextAccessor
)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<string> RegisterDriverAsync(RegisterDriverDto registerDriverDto)
        {
           
            string currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var driverData = _mapper.Map<DriverData>(registerDriverDto);
            driverData.CreatedAt = DateTime.UtcNow;
            driverData.UpdatedAt = DateTime.UtcNow;
            driverData.UserId = currentUserId;

            var driverId = await _driverRepository.AddAsync(driverData);

            var user = await _userRepository.GetByIdAsync(driverData.UserId);
            if (user != null)
            {
                user.Role = UserRole.Driver;
                await _userRepository.UpdateAsync(user);
                return driverId;

            }
            else
            {
                return null;
            }
        }

    }
}
