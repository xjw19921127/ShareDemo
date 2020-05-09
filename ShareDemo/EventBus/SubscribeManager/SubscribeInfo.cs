using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.SubscribeManager
{
    public class SubscribeInfo
    {
        public bool IsDynamic { get; }
        public Type HandlerType { get; }

        public SubscribeInfo(bool isDynamic, Type handlerType)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
        }

        public static SubscribeInfo Dynamic(Type handlerType) 
        {
            return new SubscribeInfo(true, handlerType);
        }

        public static SubscribeInfo Typed(Type handlerType) 
        {
            return new SubscribeInfo(false, handlerType);
        }
    }
}
