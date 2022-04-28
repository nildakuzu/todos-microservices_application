using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagementService.Api.Request.Models;
using UserManagementService.Api.Services.Interfaces;

namespace UserManagementService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var response = await authenticationService.Login(loginRequestModel);

            return Ok(response);
        }
    }
}
