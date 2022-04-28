using System.Threading.Tasks;
using UserManagementService.Api.Request.Models;
using UserManagementService.Api.Response.Models;

namespace UserManagementService.Api.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel);
    }
}
