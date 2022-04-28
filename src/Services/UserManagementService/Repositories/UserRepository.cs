using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.Tasks;
using UserManagementService.Api.Interfaces.Repositories;
using UserManagementService.Api.Models.Request;

namespace UserManagementService.Api.Interfaces
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _loggerFactory;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public UserRepository(ILoggerFactory loggerFactory, ConnectionMultiplexer connectionMultiplexer)
        {
            _loggerFactory = loggerFactory.CreateLogger<UserRepository>();
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }


        public async Task<bool> CreateUser(CreateUserRequestModel createUserRequestModel)
        {
            var alreadyCreated = await _database.StringGetAsync(createUserRequestModel.User.UserName);

            if (alreadyCreated.HasValue)
            {
                _loggerFactory.LogInformation("User is already created.");

                return false;
            }

            var created = await _database.StringSetAsync(createUserRequestModel.User.UserName, JsonConvert.SerializeObject(createUserRequestModel.User));

            if (!created)
            {
                _loggerFactory.LogError($"A problem is occured when {createUserRequestModel.User.UserName} user is created");
                
                return false;
            }

            _loggerFactory.LogInformation("User is created.");

            return true;
        }
    }
}
