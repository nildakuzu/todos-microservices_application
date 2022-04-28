using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using UserManagementService.Api.Services.Interfaces;

namespace UserManagementService.Api.Services
{
    public class LocalConnectionMultiplexer : ILocalConnectionMultiplexer
    {
        private readonly IConfiguration configuration;

        public LocalConnectionMultiplexer(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public ConnectionMultiplexer GetConnectionMultiplexer()
        {
            var redisConfigUrl = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
            redisConfigUrl.ResolveDns = true;

            return ConnectionMultiplexer.Connect(redisConfigUrl);
        }
    }
}
