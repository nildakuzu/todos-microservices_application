using EventBus.Base.Events;
using System.Threading.Tasks;

namespace EventBus.Base.Abstraction
{
    public interface IIntegrationEventHandler<IIntegrationEvent> where IIntegrationEvent : IntegrationEvent
    {
        Task Handle(IIntegrationEvent @event);
    }
}
