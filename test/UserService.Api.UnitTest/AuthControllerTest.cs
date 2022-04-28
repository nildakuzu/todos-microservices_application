using Microsoft.Extensions.Configuration;
using Moq;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using UserManagementService.Api.Controllers;
using UserManagementService.Api.Request.Models;
using UserManagementService.Api.Services;
using UserManagementService.Api.Services.Interfaces;
using Xunit;

namespace UserService.Api.UnitTest
{
    public class AuthControllerTest
    {
        private readonly Mock<IConfiguration> configuration_Mock;
        private readonly Mock<IDatabase> database_Mock;
        private readonly Mock<IAuthenticationService> authenticationService_Mock;

        public AuthControllerTest()
        {
            authenticationService_Mock = new Mock<IAuthenticationService>();
            configuration_Mock = new Mock<IConfiguration>();
            database_Mock = new Mock<IDatabase>();
        }

        [Fact]
        public void throw_constuctor_parameter_is_null()
        {

            Assert.Throws<ArgumentNullException>(() => new AuthController(null));
        }

        [Fact]
        public void check_hearbet_login_method()
        {
            var repsonse = new AuthController(authenticationService_Mock.Object).Login(It.IsAny<LoginRequestModel>());

            //if it does not throw, login page can handle requests
            Assert.True(true);
        }
    }
}