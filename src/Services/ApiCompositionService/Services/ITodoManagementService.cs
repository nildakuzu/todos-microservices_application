using System.Net.Http;
using System.Threading.Tasks;
using ApiCompositionService.Models;

namespace ApiCompositionService.Services
{
    public interface ITodoManagementService
    {
        Task<HttpResponseMessage> CreateTodo(TodoModel todoModel, string bearerToken);
    }
}
