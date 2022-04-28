using ApiCompositionService.Models;
using ApiCompositionService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiCompositionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCompositeController : ControllerBase
    {
        private readonly IGroupManagementService groupManagementService;
        private readonly ITodoManagementService todoManagementService;

        public ApiCompositeController(IGroupManagementService groupManagementService, ITodoManagementService todoManagementService)
        {
            this.groupManagementService = groupManagementService;
            this.todoManagementService = todoManagementService;
        }

        [HttpPost("CreateTodo")]
        [ProducesResponseType(typeof(TodoModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<HttpResponseMessage>> CreateTodo([FromBody] TodoModel todo)
        {
            var bearerToken = Request.Headers[HeaderNames.Authorization];

            var groupList = await groupManagementService.GetUserGroups(todo.UserName, bearerToken);

            if (groupList.Any(s => s.GroupName == todo.GroupName) == false)
            {
                return BadRequest();
            }

            var responseMessage = todoManagementService.CreateTodo(todo, bearerToken);

            return await responseMessage;
        }
    }
}
