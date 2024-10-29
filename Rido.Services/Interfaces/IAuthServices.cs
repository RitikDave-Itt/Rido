using Rido.Model.Requests;
using Rido.Model.Responses;
using Rido.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IAuthServices
    {
        public Task<LoginResponseDto> LoginUserAsync(LoginUserDto loginUserDto);
        public Task<string> RegisterUserAsync(RegisterUserDto userDto ,RegisterDriverDto driverDto);

        Task<(bool IsValid, string? RefreshToken, string? AccessToken)> VerifyAndGenerateRefreshToken(string token);


    }
}
