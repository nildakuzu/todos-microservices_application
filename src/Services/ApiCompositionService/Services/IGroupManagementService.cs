using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCompositionService.Models;

namespace ApiCompositionService.Services
{
    public interface IGroupManagementService
    {
        Task<IEnumerable<GroupModel>> GetUserGroups(string userName, string bearerToken);
    }
}
