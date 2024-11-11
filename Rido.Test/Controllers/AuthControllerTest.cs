//using Xunit;
//using Moq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Rido.Web.Controllers;
//using Rido.Services.Interfaces;
//using Rido.Model.Requests;
//using Rido.Data.Entities;
//using Rido.Model.Enums;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Security.Claims;

//namespace Rido.Web.Tests.Controllers
//{
//    public class AuthControllerTests
//    {
//        private readonly Mock<IAuthServices> _mockAuthService;
//        private readonly Mock<IBaseService<DriverData>> _mockDriverService;
//        private readonly Mock<IBaseService<User>> _mockUserService;
//        private readonly Mock<IMapper> _mockMapper;
//        private readonly Mock<ILogger<AuthController>> _mockLogger;
//        private readonly Mock<IServiceProvider> _mockServiceProvider;
//        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
//        private readonly AuthController _controller;

//        public AuthControllerTests()
//        {
//            _mockAuthService = new Mock<IAuthServices>();
//            _mockDriverService = new Mock<IBaseService<DriverData>>();
//            _mockUserService = new Mock<IBaseService<User>>();
//            _mockMapper = new Mock<IMapper>();
//            _mockLogger = new Mock<ILogger<AuthController>>();
//            _mockServiceProvider = new Mock<IServiceProvider>();
//            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

//            _controller = new AuthController(
//                _mockAuthService.Object,
//                null,
//                _mockDriverService.Object,
//                _mockServiceProvider.Object,
//                _mockUserService.Object
//            );

//            var httpContext = new DefaultHttpContext();
//            _controller.ControllerContext = new ControllerContext
//            {
//                HttpContext = httpContext
//            };
//        }

//        [Fact]
//        public async Task RegisterUser_ShouldReturnOk_WhenRegistrationIsSuccessful()
//        {
//            var request = new RegisterUserDto
//            {
//                FirstName = "John",
//                LastName = "Doe",
//                Gender = Gender.Male,
//                PhoneNumber = "1234567890",
//                Email = "john.doe@example.com",
//                PasswordHash = "StrongP@ssw0rd!",
//                Role = UserRole.User
//            };
//            var userDto = new RegisterUserDto { Role = UserRole.User };

//            _mockMapper.Setup(m => m.Map<RegisterUserDto>(It.IsAny<RegisterUserDto>())).Returns(userDto);
//            _mockAuthService.Setup(s => s.RegisterUserAsync(userDto, null)).ReturnsAsync("Success");

//            var result = await _controller.RegisterUser(request);

//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);
//            Assert.True(((dynamic)okResult.Value).success);
//        }

//        [Fact]
//        public async Task RegisterUser_ShouldReturnServerError_WhenRegistrationFails()
//        {
//            var request = new RegisterUserDto
//            {
//                FirstName = "Jane",
//                LastName = "Doe",
//                Gender = Gender.Female,
//                PhoneNumber = "9876543210",
//                Email = "jane.doe@example.com",
//                PasswordHash = "WeakP@ssword",
//                Role = UserRole.User
//            };
//            var userDto = new RegisterUserDto { Role = UserRole.User };

//            _mockMapper.Setup(m => m.Map<RegisterUserDto>(It.IsAny<RegisterUserDto>())).Returns(userDto);
//            _mockAuthService.Setup(s => s.RegisterUserAsync(userDto, null)).ReturnsAsync(string.Empty);

//            var result = await _controller.RegisterUser(request);

//            var statusCodeResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(500, statusCodeResult.StatusCode);
//            Assert.Equal("An error occurred while creating the user.", statusCodeResult.Value);
//        }

//        [Fact]
//        public async Task LoginUser_ShouldReturnOk_WhenLoginIsSuccessful()
//        {
//            var loginDto = new LoginUserDto { Email = "test@example.com", Password = "ValidP@ss123" };
//            var loginResult = new
//            {
//                Success = true,
//                Token = "AccessToken",
//                RefreshToken = "RefreshToken",
//                User = new User { Id = "1", Email = "test@example.com" }
//            };

//            _mockAuthService.Setup(s => s.LoginUserAsync(loginDto)).ReturnsAsync(loginResult);

//            var result = await _controller.LoginUser(loginDto);

//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var resultValue = Assert.IsAssignableFrom<dynamic>(okResult.Value);
//            Assert.Equal("AccessToken", resultValue.AccessToken);
//            Assert.Equal("RefreshToken", resultValue.RefreshToken);
//            Assert.Equal("test@example.com", resultValue.User.Email);
//        }

//        [Fact]
//        public async Task LoginUser_ShouldReturnUnauthorized_WhenLoginFails()
//        {
//            var loginDto = new LoginUserDto { Email = "invalid@example.com", Password = "WrongPassword!" };
//            var loginResult = new { Success = false };

//            _mockAuthService.Setup(s => s.LoginUserAsync(loginDto)).ReturnsAsync(loginResult);

//            var result = await _controller.LoginUser(loginDto);

//            var unauthorizedResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(401, unauthorizedResult.StatusCode);
//            Assert.Equal("Email or Password Invalid!", unauthorizedResult.Value);
//        }

//        [Fact]
//        public async Task RefreshAndVerifyToken_ShouldReturnOk_WhenTokenIsValid()
//        {
//            var authHeader = "Bearer refreshToken";
//            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = authHeader;
//            var claims = new List<Claim> { new Claim("refreshToken", "validRefreshToken") };
//            var identity = new ClaimsIdentity(claims);
//            var principal = new ClaimsPrincipal(identity);
//            _controller.HttpContext.User = principal;

//            _mockAuthService.Setup(s => s.VerifyAndGenerateRefreshToken("validRefreshToken"))
//                .ReturnsAsync((true, "newRefreshToken", "newAccessToken"));

//            var result = await _controller.RefreshAndVerifyToken();

//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var resultValue = Assert.IsAssignableFrom<dynamic>(okResult.Value);
//            Assert.True(resultValue.IsValid);
//            Assert.Equal("newRefreshToken", resultValue.RefreshToken);
//            Assert.Equal("newAccessToken", resultValue.AccessToken);
//        }

//        [Fact]
//        public async Task RefreshAndVerifyToken_ShouldReturnUnauthorized_WhenTokenIsInvalid()
//        {
//            var authHeader = "Bearer invalidToken";
//            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = authHeader;
//            var claims = new List<Claim> { new Claim("refreshToken", "invalidTokenValue") };
//            var identity = new ClaimsIdentity(claims);
//            var principal = new ClaimsPrincipal(identity);
//            _controller.HttpContext.User = principal;

//            _mockAuthService.Setup(s => s.VerifyAndGenerateRefreshToken("invalidTokenValue"))
//                .ReturnsAsync((false, null, null));

//            var result = await _controller.RefreshAndVerifyToken();

//            var unauthorizedResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(401, unauthorizedResult.StatusCode);
//            Assert.Equal("Refresh Token Expired Or Not Found", unauthorizedResult.Value);
//        }
//    }
//}
