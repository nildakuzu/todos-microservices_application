using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Base.Consants;
using EventBus.RabbitMQ;
using System;

namespace EventBus.Factory
{
    public class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                EventBusType.RabbitMQ => new EventBusRabbitMQ(serviceProvider, config),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
