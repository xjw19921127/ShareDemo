using Autofac;
using EventBus.Absratctions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventBusRedis
{
    public class EventBusRedis : IEventBus, IDisposable
    {
        private readonly IRedisConnection redisConnection;
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILogger<EventBusRedis> logger;
        private readonly IEventBusSubscribeManager eventBusSubscribeManager;

        public EventBusRedis(IRedisConnection redisConnection,ILifetimeScope lifetimeScope, ILogger<EventBusRedis> logger, IEventBusSubscribeManager eventBusSubscribeManager)
        {
            this.redisConnection = redisConnection;
            this.lifetimeScope = lifetimeScope;
            this.logger = logger;
            this.eventBusSubscribeManager = eventBusSubscribeManager;
        }

        public void Dispose()
        {
            if(redisConnection != null)
                redisConnection.Dispose();
            eventBusSubscribeManager.Clear();
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!redisConnection.IsConnected)
                redisConnection.TryConnect();

            var eventName = @event.GetType().Name;

            var multiplexer = redisConnection.GetInstance();
            ISubscriber sub = multiplexer.GetSubscriber();
            var message = JsonConvert.SerializeObject(@event);
            sub.Publish(eventName, message);
        }

        public void Subscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            var eventName = eventBusSubscribeManager.GetEventKey<E>();
            DoInternalSubscribe(eventName);
            eventBusSubscribeManager.AddSubscribe<E, EH>();
        }

        private void DoInternalSubscribe(string eventName)
        {
            if (!eventBusSubscribeManager.HasSubscribeForEvent(eventName))
            {
                if (!redisConnection.IsConnected)
                    redisConnection.TryConnect();

                if (redisConnection.IsConnected)
                {
                    var multiplexer = redisConnection.GetInstance();
                    ISubscriber sub = multiplexer.GetSubscriber();
                    sub.Subscribe(eventName, Consumer_ReceivedAsync);
                }
            }
        }

        private void Consumer_ReceivedAsync(RedisChannel eventName, RedisValue message)
        {
            try
            {
                ProcessEvent(eventName, message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (eventBusSubscribeManager.HasSubscribeForEvent(eventName))
            {
                using (var scope = lifetimeScope.BeginLifetimeScope())
                {
                    var subscribes = eventBusSubscribeManager.GetHandlersForEvent(eventName);
                    foreach (var subscribe in subscribes)
                    {
                        var handler = scope.ResolveOptional(subscribe.HandlerType);
                        if (handler == null) continue;
                        var eventType = eventBusSubscribeManager.GetEventTypeByName(eventName);
                        if (eventType == null) continue;
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
        }

        public void UnSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            eventBusSubscribeManager.RemoveSubscribe<E, EH>();

            var eventName = eventBusSubscribeManager.GetEventKey<E>();
            if (!eventBusSubscribeManager.HasSubscribeForEvent(eventName)) 
            {
                if (!redisConnection.IsConnected)
                    redisConnection.TryConnect();

                var multiplexer = redisConnection.GetInstance();
                ISubscriber sub = multiplexer.GetSubscriber();
                sub.Unsubscribe(eventName);
            }
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            DoInternalSubscribe(eventName);
            eventBusSubscribeManager.AddDynamicSubscription<TH>(eventName);
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            eventBusSubscribeManager.RemoveDynamicSubscription<TH>(eventName);

            if (!eventBusSubscribeManager.HasSubscribeForEvent(eventName))
            {
                if (!redisConnection.IsConnected)
                    redisConnection.TryConnect();

                var multiplexer = redisConnection.GetInstance();
                ISubscriber sub = multiplexer.GetSubscriber();
                sub.Unsubscribe(eventName);
            }
        }
    }
}
