using System.Threading.Tasks;
using UserManagementService.Api.Models.Request;

namespace UserManagementService.Api.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(CreateUserRequestModel createUserRequestModel);
    }
}
