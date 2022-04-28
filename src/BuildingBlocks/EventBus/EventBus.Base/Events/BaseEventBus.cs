using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly IEventBusSubscriptionManager EventBusSubscriptionManager;
        public EventBusConfig EventBusConfig;

        public BaseEventBus(IServiceProvider serviceProvider, EventBusConfig eventBusConfig)
        {
            ServiceProvider = serviceProvider;
            EventBusSubscriptionManager = new InMemoryEventBusSubscriptionManager();
            EventBusConfig = eventBusConfig;
        }

        public virtual void Dispose()
        {
            EventBusConfig = null;
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            var processed = false;

            if (EventBusSubscriptionManager.HasSubscriptionForEvent(eventName))
            {
                var subscriptions = EventBusSubscriptionManager.GetHandlersForEvent(eventName);

                using (var scope = ServiceProvider.CreateScope())
                {
                    foreach (var subscription in subscriptions)
                    {
                        var handler = ServiceProvider.GetService(subscription.HandlerType);

                        if (handler == null)
                            continue;

                        var eventType =  EventBusSubscriptionManager.GetEventTypeByName($"{eventName}");

                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }

                processed = true;
            }

            return processed;
        }

        public abstract void Publish(IntegrationEvent @event);

        public abstract void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;


        public abstract void UnSubsribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
