using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
