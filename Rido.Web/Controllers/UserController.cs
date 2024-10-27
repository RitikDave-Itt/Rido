using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rido.Data.Entities;
using Rido.Services;
using Rido.Services.Interfaces;

namespace Rido.Web.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]


    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            var user = await _userService.GetUser();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
