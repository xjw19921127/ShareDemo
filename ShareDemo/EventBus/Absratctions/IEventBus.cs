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

        void UnSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>;
    }
}
