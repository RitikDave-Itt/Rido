using Rido.Common.Models.Requests;
using Rido.Common.Models.Responses;
using Rido.Data.Enums;
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
