using Rido.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Tests.Controller
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthServices> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthServices>();
            _controller = new AuthController(_mockAuthService.Object, null, null, null, null);
        }

        [Fact]
        public async Task RegisterUser_ReturnsOk_OnSuccessfulRegistration()
        {
            var request = new { Email = "test@example.com", Password = "password", Role = UserRole.User };
            _mockAuthService.Setup(s => s.RegisterUserAsync(It.IsAny<RegisterUserDto>(), null))
                .ReturnsAsync("success");

            var result = await _controller.RegisterUser(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value?.GetType().GetProperty("success")?.GetValue(okResult.Value));
        }

        [Fact]
        public async Task RegisterUser_ReturnsServerError_WhenRegistrationFails()
        {
            var request = new { Email = "test@example.com", Password = "password", Role = UserRole.User };
            _mockAuthService.Setup(s => s.RegisterUserAsync(It.IsAny<RegisterUserDto>(), null))
                .ReturnsAsync(string.Empty);

            var result = await _controller.RegisterUser(request);

            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task LoginUser_ReturnsOk_OnSuccessfulLogin()
        {
            var loginDto = new LoginUserDto { Email = "test@example.com", Password = "password" };
            var loginResult = new LoginResult { Success = true, Token = "accessToken", RefreshToken = "refreshToken" };
            _mockAuthService.Setup(s => s.LoginUserAsync(loginDto)).ReturnsAsync(loginResult);

            var result = await _controller.LoginUser(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task RefreshAndVerifyToken_ReturnsUnauthorized_WhenTokenIsInvalid()
        {
            _mockAuthService.Setup(s => s.VerifyAndGenerateRefreshToken(It.IsAny<string>()))
                .ReturnsAsync((false, null, null));

            var result = await _controller.RefreshAndVerifyToken();

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Refresh Token Expired Or Not Found", unauthorizedResult.Value);
        }

        [Fact]
        public async Task RefreshAndVerifyToken_ReturnsOk_WhenTokenIsValid()
        {
            string validRefreshToken = "validRefreshToken";
            _mockAuthService.Setup(s => s.VerifyAndGenerateRefreshToken(validRefreshToken))
                .ReturnsAsync((true, validRefreshToken, "newAccessToken"));

            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer " + validRefreshToken;

            var result = await _controller.RefreshAndVerifyToken();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
