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
    public class AuthService : IAuthServices
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<DriverData> _driverRepository;

        private readonly IUserRepository _userSpecificRepository;
        private readonly JwtUtil _jwtService;
        private readonly IMapper _mapper;

        private readonly IBaseRepository<OneTimePassword> _otpRepository;

        public AuthService(IBaseRepository<User> userRepository,IBaseRepository<DriverData> driverRepository, IUserRepository userSpecificRepository, JwtUtil jwtService, IMapper mapper,IBaseRepository<OneTimePassword> otpRepository)
        {
            _userRepository = userRepository;
            _userSpecificRepository = userSpecificRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _otpRepository = otpRepository;
            _driverRepository = driverRepository;


        }

        public async Task<LoginResponse> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _userSpecificRepository.GetUserByEmail(loginUserDto.Email);

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

        public async Task<string> RegisterUserAsync(RegisterUserDto userDto,RegisterDriverDto driverDto)
        {

            userDto.PasswordHash = PasswordHasher.HashPassword(userDto.PasswordHash);
            var user = _mapper.Map<User>(userDto);


            var registeredUser = await _userRepository.AddAsync(user);

            if (user.Role == UserRole.Driver)
            {

                driverDto.UserId = registeredUser.Id;

                var drievr = _mapper.Map<DriverData>(driverDto);

                var saveDriver = await _driverRepository.AddAsync(drievr);
            }


            

            return  registeredUser.Id;
        }

        public async Task<bool> VerifyOTP(string otp, string rideRequestId)
        {
           
         return StringUtils.MatchOTP(rideRequestId, otp);
        }


    }
}
