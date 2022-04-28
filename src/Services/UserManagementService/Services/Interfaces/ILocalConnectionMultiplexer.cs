using StackExchange.Redis;

namespace UserManagementService.Api.Services.Interfaces
{
    public interface ILocalConnectionMultiplexer
    {
        ConnectionMultiplexer GetConnectionMultiplexer();
    }
}
