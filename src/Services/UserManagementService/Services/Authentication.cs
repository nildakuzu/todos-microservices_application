using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.Api.Models;
using UserManagementService.Api.Request.Models;
using UserManagementService.Api.Response.Models;
using UserManagementService.Api.Services.Interfaces;

namespace UserManagementService.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public AuthenticationService(IConfiguration configuration, ConnectionMultiplexer connectionMultiplexer)
        {
            _configuration = configuration;
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel)
        {
            var user = await _database.StringGetAsync(loginRequestModel.UserName);

            if (!user.HasValue)
            {
                return await Task.FromResult(new LoginResponseModel("User is not defined;"));
            }

            var deserializedUser = JsonConvert.DeserializeObject<UserModel>(user);

            if (deserializedUser.Password != loginRequestModel.Password)
            {
                return await Task.FromResult(new LoginResponseModel("UserName/Pasword is correct."));
            }

            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier,deserializedUser.UserName),
                new Claim(ClaimTypes.Name,deserializedUser.Name),
                new Claim(ClaimTypes.Surname,deserializedUser.Surname),
                new Claim(ClaimTypes.Email,deserializedUser.Email),
            };

            var authConfigStr = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthConfigStr"]));

            var credential = new SigningCredentials(authConfigStr, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.Now.AddDays(30);

            var jwtToken = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: credential);

            var seriliazedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var response = new LoginResponseModel()
            {
                UserName = loginRequestModel.UserName,
                Token = seriliazedJwtToken
            };


            return await Task.FromResult(response);

        }
    }
}
