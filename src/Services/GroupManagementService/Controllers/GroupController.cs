using GroupManagementService.Api.Entities;
using GroupManagementService.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GroupManagementService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _repository;

        public GroupController(IGroupRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        [HttpGet("GetAll", Name = "GetAll")]
        [ProducesResponseType(typeof(Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Group>>> GetAll()
        {
            var allList = await _repository.GetAll();

            return Ok(allList);
        }

        [HttpGet("GetUserGroups/{userName}", Name = "GetUserGroups")]
        [ProducesResponseType(typeof(Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Group>>> GetUserGroups(string userName)
        {
            var userGroupList = await _repository.GetUserGroups(userName);

            return Ok(userGroupList);
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Group>> Create([FromBody] Group group)
        {
            await _repository.Create(group);

            return CreatedAtRoute("Get", new { groupname = group.GroupName }, group);
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Group>> Update([FromBody] Group Group)
        {
            return Ok(await _repository.Update(Group));
        }

        [HttpDelete("DeleteUserGroups/{userName}", Name = "DeleteUserGroup")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteUserGroup(string userName)
        {
            return Ok(await _repository.DeleteUserGroup(userName));
        }

        [HttpDelete("Delete/{groupName}", Name = "Delete")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Delete(string groupName)
        {
            return Ok(await _repository.Delete(groupName));
        }

        [HttpGet("Get/{groupName}", Name = "Get")]
        [ProducesResponseType(typeof(Group), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Group>> Get(string groupName)
        {
            var userGroup = await _repository.Get(groupName);

            return Ok(userGroup);
        }
    }
}
