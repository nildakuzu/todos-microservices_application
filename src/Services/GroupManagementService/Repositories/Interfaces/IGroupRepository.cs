using GroupManagementService.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupManagementService.Api.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetUserGroups(string userName);
        Task<Group> Get(string groupName);
        Task<bool> Create(Group group);
        Task<bool> Update(Group group);
        Task<bool> DeleteUserGroup(string userName);
        Task<bool> Delete(string groupName);
    }
}
