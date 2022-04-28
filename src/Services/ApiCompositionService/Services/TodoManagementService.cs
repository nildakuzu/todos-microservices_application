using ApiCompositionService.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiCompositionService.Services
{
    public class TodoManagementService : ITodoManagementService
    {
        private readonly HttpClient _client;

        public TodoManagementService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<HttpResponseMessage> CreateTodo(TodoModel todoModel, string bearerToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{_client.BaseAddress}api/Todo/Create"));
            var seriliazedModel = JsonConvert.SerializeObject(todoModel);
            request.Content = new StringContent(seriliazedModel, Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var response = await _client.SendAsync(request);

            return response;
        }

    }
}
