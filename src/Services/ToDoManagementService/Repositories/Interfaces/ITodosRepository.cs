using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoManagementService.Api.Entities;

namespace ToDoManagementService.Api.Repositories.Interfaces
{
    public interface ITodosRepository
    {
        Task<IEnumerable<Todo>> GetUserTodos(string userName);
        Task<IEnumerable<Todo>> FilterTodos(Todo todo);
        Task<Todo> Get(int id);
        Task<bool> Create(Todo todo);
        Task<bool> Update(Todo todo);
        Task<bool> Delete(int id);
        Task<bool> DeleteUserTodos(string userName);
    }
}
