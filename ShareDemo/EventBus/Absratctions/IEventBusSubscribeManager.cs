using EventBus.Events;
using EventBus.SubscribeManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Absratctions
{
    public interface IEventBusSubscribeManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;
        void AddSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;

        void AddDynamicSubscription<TH>(string eventName)
           where TH : IDynamicIntegrationEventHandler;

        void RemoveSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;

        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        bool HasSubscribeForEvent<E>()
            where E : IntegrationEvent;
        bool HasSubscribeForEvent(string eventName);

        IEnumerable<SubscribeInfo> GetHandlersForEvent<E>()
            where E : IntegrationEvent;

        IEnumerable<SubscribeInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<E>()
            where E : IntegrationEvent;

        Type GetEventTypeByName(string eventName);
        void Clear();
    }
}
