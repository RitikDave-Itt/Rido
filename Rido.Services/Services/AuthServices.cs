using Rido.Model.Requests;
using Rido.Common.Utils;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using System.Threading.Tasks;
using Rido.Services.Interfaces;
using Rido.Model.Responses;
using AutoMapper;
using Rido.Data.Contexts;
using Rido.Model.Enums;
using System.Text.RegularExpressions;
using Rido.Common.Secrets;
using Microsoft.Extensions.Options;
using Rido.Model.Attributes;


namespace Rido.Services
{
    public class AuthService : BaseService<User>, IAuthServices
    {
        private readonly IBaseRepository<DriverData> _driverRepository;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
        private readonly IBaseRepository<RideRequest> _rideRequestRepository;





        private readonly JwtUtil _jwtService;
        private readonly JwtSettings _jwtSettings;



        public AuthService(IBaseRepository<DriverData> driverRepository
            , JwtUtil jwtService, IServiceProvider serviceProvider
            , IUserRepository userRepository
            , IOptions<JwtSettings> jwtSettings
            , IBaseRepository<User> userBaseRepository
            , IBaseRepository<RefreshToken> refreshTokenRepository
            , IBaseRepository<RideRequest> rideRequestRepository)

            : base(serviceProvider)
        {
            _jwtService = jwtService;
            _driverRepository = driverRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
            _rideRequestRepository = rideRequestRepository;


        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
        {

            var user = await _repository.FindAsync(u => u.Email == loginUserDto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, loginUserDto.Password))
            {
                return new LoginResponseDto() { Success = false };
            }


            var existingRefreshToken = await _refreshTokenRepository.FindAsync(rt => rt.UserId == user.Id);

            RefreshToken refreshToken;

            string token = Guid.NewGuid().ToString();

            if (existingRefreshToken == null)
            {
                refreshToken = new RefreshToken
                {
                    Token = token,
                    UserId = user.Id,
                    Expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
                };
                var result = await _refreshTokenRepository.AddAsync(refreshToken);
                refreshToken = result;
            }
            else
            {
                existingRefreshToken.Token = token;
                existingRefreshToken.Expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);

                bool updateSuccess = await _refreshTokenRepository.UpdateAsync(existingRefreshToken);

                if (!updateSuccess)
                {
                    throw new Exception("Unable to update Refresh Token");
                }

                refreshToken = existingRefreshToken;
            }

            string refreshTokenString = _jwtService.GenerateRefreshToken(user.Id, user.Email, refreshToken.Token);


            return new LoginResponseDto
            {
                Success = true,
                Token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.Role),
                RefreshToken = refreshTokenString,
                User = new
                {

                    Name = $"{user?.LastName} {user?.LastName}",
                    email = user?.Email,
                    Role = user?.Role.ToString(),
                    profileImage = user?.ProfileImage?.Base64String,
                    Gender = user?.Gender.ToString(),
                    PhoneNumber = user?.PhoneNumber.ToString(),
                    VehicleType = user?.DriverData?.VehicleType.ToString(),

                }
            };
        }
    
        


        public async Task<string> RegisterUserAsync(RegisterUserDto userDto, RegisterDriverDto driverDto = null)
        {

            userDto.PasswordHash = PasswordHasher.HashPassword(userDto.PasswordHash);
            var user = _mapper.Map<User>(userDto);

            DriverData driver = new DriverData();
            if (driverDto != null) {

                driver = _mapper.Map<DriverData>(driverDto);

                user.DriverData = driver;

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

            user.Wallet = wallet;


            var registeredUser = await _repository.AddAsync(user);



            return registeredUser.Id;
        }

      

        public async Task<(bool IsValid , string? RefreshToken  , string? AccessToken )> VerifyAndGenerateRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(t => t.Token==token, t=>t.User);


            if(refreshToken == null || refreshToken.IsRevoked || refreshToken.Expiry < DateTime.UtcNow||refreshToken.User==null) 
            {
                return (
                    IsValid : false,
                    RefreshToken : null,
                    AccessToken:null
                    );
            }
            var newToken = Guid.NewGuid().ToString();

            var JwtToken = _jwtService.GenerateToken(refreshToken.UserId, refreshToken.User.Email, refreshToken.User.Role);
            refreshToken.IsRevoked = false;
            refreshToken.Token = newToken;

            var jwtRefreshToken = _jwtService.GenerateRefreshToken(refreshToken.User.Id, refreshToken.User.Email, newToken);

            var saveRefreshToken = await _refreshTokenRepository.UpdateAsync(refreshToken);

            if (!saveRefreshToken)
            {
                throw new InvalidOperationException("Umable To Update Refrsh Tolen");
            }

            return (
                IsValid: true,
                RefreshToken: jwtRefreshToken,
                AccessToken:JwtToken
                );
            
            

        } 

    }
}
