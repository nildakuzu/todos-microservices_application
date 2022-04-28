using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementService.Api.Interfaces.Repositories;
using UserManagementService.Api.Models.Request;

namespace UserManagementService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("Create")]
        public async Task<bool> Create([FromBody] CreateUserRequestModel createUserRequestModel)
        {
            return await Task.Run(() => userRepository.CreateUser(createUserRequestModel));
        }
    }
}
