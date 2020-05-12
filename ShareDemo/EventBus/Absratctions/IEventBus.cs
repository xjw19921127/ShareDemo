using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Absratctions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
        void Subscribe<E, EH>() 
            where E : IntegrationEvent 
            where EH : IIntegrationEventHandler<E>;
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        void UnSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;

        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;
    }
}
