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
using Rido.Common.Secrets;
using Microsoft.Extensions.Options;


namespace Rido.Services
{
    public class AuthService : BaseService<User>, IAuthServices
    {
        private readonly IBaseRepository<DriverData> _driverRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
        private readonly IBaseRepository<User> _userBaseRepository;
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
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
            _userBaseRepository = userBaseRepository;
            _rideRequestRepository = rideRequestRepository;


        }

        public async Task<LoginResponse> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _repository.FindAsync(u => u.Email == loginUserDto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, loginUserDto.Password))
            {
                return new LoginResponse { Success = false, Token = null };
            }

            var existingRefreshToken = await _refreshTokenRepository.FindAsync(rt => rt.UserId == user.Id);

            RefreshToken refreshToken;

            if (existingRefreshToken == null)
            {
                refreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Expiry = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
                };
                var result = await _refreshTokenRepository.AddAsync(refreshToken);
                refreshToken = result;
            }
            else
            {
                existingRefreshToken.Token = Guid.NewGuid().ToString();
                existingRefreshToken.Expiry = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpiryInDays);

                bool updateSuccess = await _refreshTokenRepository.UpdateAsync(existingRefreshToken);

                if (!updateSuccess)
                {
                    throw new Exception("Unable to update Refresh Token");
                }

                refreshToken = existingRefreshToken;
            }

            return new LoginResponse
            {
                Success = true,
                Token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.Role),
                RefreshToken = refreshToken?.Token,
            };
        }


        public async Task<string> RegisterUserAsync(RegisterUserDto userDto, RegisterDriverDto driverDto = null)
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

            var registeredUser = await _userRepository.CreateUser(user, wallet, driver);



            return registeredUser;
        }

        public async Task<OTPVerificationStatus> VerifyOTP(string otp, string rideRequestId)
        {
            var UserId = GetCurrentUserId();
            bool otpVerify = OtpUtils.MatchOTP(rideRequestId, otp);


            if (!otpVerify) { 


                return OTPVerificationStatus.InvalidOTP;
            
            }

            var rideRequest  = await _rideRequestRepository.GetByIdAsync(rideRequestId);

            if (rideRequest == null||rideRequest.Status!=RideRequestStatus.Accepted||rideRequest.DriverId!=UserId) {
                return OTPVerificationStatus.InvalidRideRequestStatus;
            }

            rideRequest.Status = RideRequestStatus.Started; 

            var update = await _rideRequestRepository.UpdateAsync(rideRequest);

                   


         return  OTPVerificationStatus.Success;
        }


        public async Task<(bool IsValid , string? RefreshToken  , string? JwtToken )> VerifyAndGenerateRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(t => t.Token==token, t=>t.User);


            if(refreshToken == null || refreshToken.IsRevoked || refreshToken.Expiry < DateTime.Now||refreshToken.User==null) 
            {
                return (
                    IsValid : false,
                    RefreshToken : null,
                    JwtToken:null
                    );
            }

            var JwtToken = _jwtService.GenerateToken(refreshToken.UserId, refreshToken.User.Email, refreshToken.User.Role);
            refreshToken.IsRevoked = false;
            refreshToken.Token = Guid.NewGuid().ToString();

            var saveRefreshToken = await _refreshTokenRepository.UpdateAsync(refreshToken);

            if (!saveRefreshToken)
            {
                throw new InvalidOperationException("Umable To Update Refrsh Tolen");
            }

            return (
                IsValid: true,
                RefreshToken: refreshToken.Token,
                JwtToken
                );
            
            

        } 

    }
}
