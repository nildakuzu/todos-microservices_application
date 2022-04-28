using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus.Base.SubManagers
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionHandlerType>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionHandlerType>>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty => _handlers.Any() == false;

        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventName<T>();

            AddSubscription(typeof(TH), eventName);


            if (_eventTypes.Contains(typeof(T)) == false)
            {
                _eventTypes.Add(typeof(T));
            }
        }

        public void Clear()
        {
            _handlers.Clear();
        }

        public Type GetEventTypeByName(string eventName)
        {
            return _eventTypes.SingleOrDefault(s => s.Name == eventName);
        }

        public string GetEventName<T>()
        {
            var eventName = typeof(T).Name;

            return eventName;
        }

        public IEnumerable<SubscriptionHandlerType> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventName<T>();

            return _handlers[eventName];
        }

        public IEnumerable<SubscriptionHandlerType> GetHandlersForEvent(string eventName)
        {
            return _handlers[eventName];
        }

        public bool HasSubscriptionForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventName<T>();

            return _handlers.ContainsKey(eventName);
        }

        public bool HasSubscriptionForEvent(string eventName)
        {
            return _handlers.ContainsKey(eventName);
        }

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventName<T>();

            if (handlerToRemove != null)
            {
                _handlers[eventName].Remove(handlerToRemove);

                if (_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);

                    var eventType = _eventTypes.SingleOrDefault(s => s.Name == eventName);

                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }

                    RaiseOnEventRemoved(eventName);
                }
            }

        }

        private void RaiseOnEventRemoved(string eventname)
        {

            var onRemovedEvent = OnEventRemoved;

            onRemovedEvent?.Invoke(this, eventname);
        }

        private SubscriptionHandlerType FindSubscriptionToRemove<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventName<T>();

            if (HasSubscriptionForEvent(eventName) == false)
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == typeof(TH));
        }

        private void AddSubscription(Type eventHandlerType, string eventName)
        {
            if (HasSubscriptionForEvent(eventName) == false)
            {
                _handlers.Add(eventName, new List<SubscriptionHandlerType>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == eventHandlerType))
            {
                throw new ArgumentException($"{eventHandlerType} is already registered for {eventName}");
            }

            _handlers[eventName].Add(SubscriptionHandlerType.GetHandlerTyped(eventHandlerType));
        }
    }
}
