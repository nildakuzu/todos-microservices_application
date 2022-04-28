using System;

namespace EventBus.Base
{
    public class SubscriptionHandlerType
    {
        public Type HandlerType { get; set; }

        public SubscriptionHandlerType(Type handlerType)
        {
            HandlerType = handlerType;
        }

        public static SubscriptionHandlerType GetHandlerTyped(Type handlerType)
        {
            return new SubscriptionHandlerType(handlerType);
        }
    }
}
