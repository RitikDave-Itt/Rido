//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Rido.Services.Interfaces;
//using Rido.Data.Entities;
//using Rido.Web.Controllers;

//namespace Rido.Test.Controllers
//{
//    public class AuthControllerTest
//    {
//        private readonly Mock<IAuthServices> _mockAuthService;
//        private readonly Mock<IBaseService<DriverData>> _mockDriverService;
//        private readonly Mock<IBaseService<User>> _mockUserService;
//        private readonly AuthController _authController;
//        public AuthControllerTest()
//        {
//            _mockAuthService = new Mock<IAuthServices>();
//            _mockDriverService = new Mock<IBaseService<DriverData>>();
//            _mockUserService = new Mock<IBaseService<User>>();

//            _authController = new AuthController(
//                _mockAuthService.Object,
//                Mock.Of<IBaseService<RideRequest>>(),          
//                _mockDriverService.Object,
//                null,       
//                _mockUserService.Object
//            );
//        }
//        //[Fact]

//        //public async Task RegisterUserTest()
//        //{
//        //    var request = new

//        //}



//    }
//}
