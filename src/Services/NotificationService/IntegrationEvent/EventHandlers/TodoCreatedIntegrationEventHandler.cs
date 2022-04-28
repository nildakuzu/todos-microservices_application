using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Api.Events;
using System.Threading.Tasks;

namespace NotificationService.Api.EventHandlers
{
    public class TodoCreatedIntegrationEventHandler : IIntegrationEventHandler<TodoCreatedIntegrationEvent>
    {
        ILogger<TodoCreatedIntegrationEvent> _logger;
        public TodoCreatedIntegrationEventHandler(ILogger<TodoCreatedIntegrationEvent> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TodoCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("---Handling todocreated integration event");

            await Task.FromResult(0);
        }
    }
}
