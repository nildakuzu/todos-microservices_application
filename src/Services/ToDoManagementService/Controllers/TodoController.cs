using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ToDoManagementService.Api.Entities;
using ToDoManagementService.Api.Events;
using ToDoManagementService.Api.Repositories.Interfaces;

namespace ToDoManagementService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodosRepository _repository;
        private readonly IEventBus eventBus;

        public TodoController(ITodosRepository repository, IEventBus eventBus)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.eventBus = eventBus;
        }

        [HttpGet("GetUserTodos/{userName}", Name = "GetUserTodos")]
        [ProducesResponseType(typeof(Todo), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Todo>>> GetUserTodos(string userName)
        {
            var userTodoList = await _repository.GetUserTodos(userName);

            return Ok(userTodoList);
        }

        [HttpPost("Filter")]
        [ProducesResponseType(typeof(Todo), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Todo>> Filter([FromBody] Todo todo)
        {
            return Ok(await _repository.FilterTodos(todo));
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(Todo), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Todo>> Create([FromBody] Todo todo)
        {
            await _repository.Create(todo);

            var eventMessage = new TodoCreatedIntegrationEvent(todo.Id, todo.UserName, todo.DueDate);

            try
            {
                eventBus.Publish(eventMessage);
            }
            catch (Exception)
            {

                throw;
            }

            return CreatedAtRoute("Get", new { id = todo.Id }, todo);
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(Todo), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Todo>> Update([FromBody] Todo todo)
        {
            return Ok(await _repository.Update(todo));
        }

        [HttpDelete("DeleteUserTodos/{userName}", Name = "DeleteUserTodos")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteUserTodos(string userName)
        {
            return Ok(await _repository.DeleteUserTodos(userName));
        }

        [HttpDelete("Delete/{id}", Name = "DeleteTodo")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            return Ok(await _repository.Delete(id));
        }

        [HttpGet("Get/{id}", Name = "Get")]
        [ProducesResponseType(typeof(Todo), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Todo>> Get(int id)
        {
            var todo = await _repository.Get(id);

            return Ok(todo);
        }
    }
}
