using EventBus.Absratctions;
using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventBus.SubscribeManager
{
    public class InMemoryEventBusSubscribeManager : IEventBusSubscribeManager
    {
        private readonly Dictionary<string, List<SubscribeInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public InMemoryEventBusSubscribeManager()
        {
            _handlers = new Dictionary<string, List<SubscribeInfo>>();
            _eventTypes = new List<Type>();
        }


        public bool IsEmpty => !_handlers.Any();

        public event EventHandler<string> OnEventRemoved;

        public void AddSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            var eventName = GetEventKey<E>();

            DoAddSubscribe(typeof(EH), eventName, false);

            if (!_eventTypes.Contains(typeof(E)))
                _eventTypes.Add(typeof(E));
        }

        private void DoAddSubscribe(Type handlerType, string eventName, bool isDynamic)
        {
            if (!_handlers.ContainsKey(eventName))
                _handlers.Add(eventName, new List<SubscribeInfo>());

            if (_handlers[eventName].Any(o => o.HandlerType == handlerType))
                throw new ArgumentException($"Handler Type: {handlerType.Name} has registered for {eventName}");

            if (isDynamic)
                _handlers[eventName].Add(SubscribeInfo.Dynamic(handlerType));
            else
                _handlers[eventName].Add(SubscribeInfo.Typed(handlerType));
        }

        public void Clear() => _handlers.Clear();

        public string GetEventKey<E>() where E : IntegrationEvent
        {
            return typeof(E).Name;
        }

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(o => o.Name == eventName);

        public IEnumerable<SubscribeInfo> GetHandlersForEvent<E>() where E : IntegrationEvent
        {
            var eventName = GetEventKey<E>();

            return GetHandlersForEvent(eventName);
        }

        public IEnumerable<SubscribeInfo> GetHandlersForEvent(string eventName)
        {
            return _handlers[eventName];
        }

        public bool HasSubscribeForEvent<E>() where E : IntegrationEvent
        {
            var eventName = GetEventKey<E>();
            return HasSubscribeForEvent(eventName);
        }

        public bool HasSubscribeForEvent(string eventName)
        {
            return _handlers.ContainsKey(eventName);
        }

        public void RemoveSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            var handlerToRemove = FindSubscribeToRemove<E, EH>();
            var eventName = GetEventKey<E>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        private void DoRemoveHandler(string eventName, SubscribeInfo subscribeInfo) 
        {
            if (subscribeInfo != null) 
            {
                _handlers[eventName].Remove(subscribeInfo);
                if (!_handlers[eventName].Any()) 
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(o => o.Name == eventName);
                    if (eventType != null)
                        _eventTypes.Remove(eventType);
                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        private void RaiseOnEventRemoved(string eventName) 
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        private SubscribeInfo FindSubscribeToRemove<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            var eventName = GetEventKey<E>();
            return DoFindSubscribe(eventName, typeof(EH));
        }

        private SubscribeInfo DoFindSubscribe(string eventName, Type handlerType) 
        {
            if (!HasSubscribeForEvent(eventName))
                return null;

            return _handlers[eventName].SingleOrDefault(o => o.HandlerType == handlerType);
        }

        public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            DoAddSubscribe(typeof(TH), eventName, isDynamic: true);
        }

        public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        private SubscribeInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            return DoFindSubscribe(eventName, typeof(TH));
        }
    }
}
