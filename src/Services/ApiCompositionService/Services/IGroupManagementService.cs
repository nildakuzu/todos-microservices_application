using ApiCompositionService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCompositionService.Services
{
    public interface IGroupManagementService
    {
        Task<IEnumerable<GroupModel>> GetUserGroups(string userName, string bearerToken);
    }
}
