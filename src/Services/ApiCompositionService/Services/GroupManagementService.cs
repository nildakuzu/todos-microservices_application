using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ApiCompositionService.Extensions;
using ApiCompositionService.Models;

namespace ApiCompositionService.Services
{
    public class GroupManagementService : IGroupManagementService
    {
        private readonly HttpClient _client;

        public GroupManagementService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<GroupModel>> GetUserGroups(string userName, string bearerToken)
        {
            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var response = await _client.GetAsync($"/api/Group/GetUserGroups/{userName}");

            return await response.ReadContentAs<List<GroupModel>>();
        }
    }
}
