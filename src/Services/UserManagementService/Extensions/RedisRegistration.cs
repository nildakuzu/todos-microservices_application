using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace UserManagementService.Api.Extensions
{
    public static class RedisRegistration
    {
        public static ConnectionMultiplexer ConfigureRedis(this IServiceProvider serviceCollection, IConfiguration configuration)
        {
            var redisConfigUrl = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
            redisConfigUrl.ResolveDns = true;

            return ConnectionMultiplexer.Connect(redisConfigUrl);

        }

    }
}
