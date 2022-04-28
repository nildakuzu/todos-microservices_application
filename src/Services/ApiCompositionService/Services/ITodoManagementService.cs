using ApiCompositionService.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiCompositionService.Services
{
    public interface ITodoManagementService
    {
        Task<HttpResponseMessage> CreateTodo(TodoModel todoModel, string bearerToken);
    }
}
