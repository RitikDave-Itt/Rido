using Rido.Common.Models.Requests;
using Rido.Common.Utils;
using Rido.Data.Entities;
using Rido.Data.Repositories.Interfaces;
using System.Threading.Tasks;
using Rido.Services.Interfaces;
using Rido.Common.Models.Responses;
using AutoMapper;


namespace Rido.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUserRepository _userSpecificRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(IRepository<User> userRepository, IUserRepository userSpecificRepository, IJwtService jwtService, IMapper mapper)
        {
            _userRepository = userRepository;
            _userSpecificRepository = userSpecificRepository;
            _jwtService = jwtService;
            _mapper = mapper;


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

        public async Task<string> RegisterUserAsync(RegisterUserDto requestDto)
        {
            var existingUser = await _userSpecificRepository.GetUserByEmail(requestDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            requestDto.PasswordHash = PasswordHasher.HashPassword(requestDto.PasswordHash);
            var user = _mapper.Map<User>(requestDto);



            return await _userRepository.AddAsync(user);
        }
    }
}
