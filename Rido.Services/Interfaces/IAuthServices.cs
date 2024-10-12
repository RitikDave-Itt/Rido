using Rido.Common.Models.Requests;
using Rido.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Services.Interfaces
{
    public interface IAuthServices
    {
        public Task<LoginResponse> LoginUserAsync(LoginUserDto loginUserDto);
        public Task<bool> RegisterUserAsync(RegisterUserDto requestDto);


    }
}
