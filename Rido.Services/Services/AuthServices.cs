using Rido.Common.Models.Requests;
using Rido.Common.Utils;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using System.Threading.Tasks;
using Rido.Services.Interfaces;
using Rido.Common.Models.Responses;
using AutoMapper;
using Rido.Data.Contexts;
using Rido.Data.Enums;
using System.Text.RegularExpressions;


namespace Rido.Services
{
    public class AuthService : BaseService<User>,IAuthServices
    {
        private readonly IBaseRepository<DriverData> _driverRepository;
        private readonly IUserRepository _userRepository;



        private readonly JwtUtil _jwtService;


        public AuthService(IBaseRepository<DriverData> driverRepository, JwtUtil jwtService,IServiceProvider serviceProvider , IUserRepository userRepository):base(serviceProvider)
        {
            _jwtService = jwtService;
            _driverRepository = driverRepository;
            _userRepository = userRepository;


        }

        public async Task<LoginResponse> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _repository.FindFirstAsync(u=>u.Email==loginUserDto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, loginUserDto.Password))
            {
                return new LoginResponse { Success = false, Token = null };
            }

            return new LoginResponse
            {
                Success = true,
                Token = _jwtService.GenerateToken(user.Id.ToString(), user.Email,user.Role)
            };
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto userDto,RegisterDriverDto driverDto = null)
        {

            userDto.PasswordHash = PasswordHasher.HashPassword(userDto.PasswordHash);
            var user = _mapper.Map<User>(userDto);

            DriverData driver = new DriverData();
            if (driverDto != null) { 
            driver = _mapper.Map<DriverData>(driverDto);

            }
            else
            {
                driver = null;
            }

            Wallet wallet = new Wallet()
            {
                Balance = 10000,
                UserId = user.Id,
                WalletStatus = WalletStatus.Active
            };

            var registeredUser = await _userRepository.CreateUser(user, wallet,driver);



            return  registeredUser;
        }

        public async Task<bool> VerifyOTP(string otp, string rideRequestId)
        {
           
         return OtpUtils.MatchOTP(rideRequestId, otp);
        }


    }
}
