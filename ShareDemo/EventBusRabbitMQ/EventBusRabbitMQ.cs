using Autofac;
using EventBus.Absratctions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        const string BROKER_NAME = "ContainerApp_Event_Bus";
        private readonly IRabbitMQConnection rabbitMQConnection;
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILogger<EventBusRabbitMQ> logger;
        private readonly IEventBusSubscribeManager eventBusSubscribeManager;
        private string queueName;
        private IModel consumerChannel;

        public EventBusRabbitMQ(IRabbitMQConnection rabbitMQConnection,ILifetimeScope lifetimeScope, ILogger<EventBusRabbitMQ> logger, IEventBusSubscribeManager eventBusSubscribeManager, string queueName = null)
        {
            this.rabbitMQConnection = rabbitMQConnection;
            this.lifetimeScope = lifetimeScope;
            this.logger = logger;
            this.eventBusSubscribeManager = eventBusSubscribeManager;
            this.queueName = queueName;
            consumerChannel = CreateConsumerChannel();
            eventBusSubscribeManager.OnEventRemoved += EventBusSubscribeManager_OnEventRemoved;
        }

        private void EventBusSubscribeManager_OnEventRemoved(object sender, string eventName)
        {
            if (!rabbitMQConnection.IsConnected)
                rabbitMQConnection.TryConnect();
            using (var channel = rabbitMQConnection.CreateModel()) 
            {
                channel.QueueUnbind(
                    queue: queueName,
                    exchange: BROKER_NAME, 
                    routingKey: eventName, 
                    arguments: null);

                if (eventBusSubscribeManager.IsEmpty) 
                {
                    queueName = string.Empty;
                    consumerChannel.Close();
                }
            }
        }

        public void Dispose()
        {
            if (consumerChannel != null)
                consumerChannel.Dispose();
            eventBusSubscribeManager.Clear();
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!rabbitMQConnection.IsConnected)
                rabbitMQConnection.TryConnect();

            var eventName = @event.GetType().Name;

            using (var channel = rabbitMQConnection.CreateModel()) 
            {
                channel.ExchangeDeclare(
                    exchange: BROKER_NAME, 
                    type: "direct", 
                    durable: false, 
                    autoDelete: false, 
                    arguments: null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; //persistent

                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties, 
                    body: body);
            }
        }

        public void Subscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            var eventName = eventBusSubscribeManager.GetEventKey<E>();
            DoInternalSubscribe(eventName);
            eventBusSubscribeManager.AddSubscribe<E, EH>();
            StartBasicConsume();
        }

        private void StartBasicConsume() 
        {
            if (consumerChannel != null) 
            {
                var consumer = new AsyncEventingBasicConsumer(consumerChannel);
                consumer.Received += Consumer_Received;

                //consumerChannel.BasicQos(prefetchCount: 0, prefetchSize: 0, global: false);

                consumerChannel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs args)
        {
            var eventName = args.RoutingKey;
            var message = Encoding.UTF8.GetString(args.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
                consumerChannel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
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

        private IModel CreateConsumerChannel() 
        {
            if (!rabbitMQConnection.IsConnected)
                rabbitMQConnection.TryConnect();

            var channel = rabbitMQConnection.CreateModel();
            channel.ExchangeDeclare(
                exchange: BROKER_NAME,
                type: "direct");

            channel.QueueDeclare(
                queue: queueName, 
                durable: true, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null);

            //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            channel.CallbackException += (sender, args) => 
            {
                consumerChannel.Dispose();
                consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };
            return channel;
        }

        private void DoInternalSubscribe(string eventName) 
        {
            if (!eventBusSubscribeManager.HasSubscribeForEvent(eventName)) 
            {
                if (!rabbitMQConnection.IsConnected)
                    rabbitMQConnection.TryConnect();

                using (var channel = rabbitMQConnection.CreateModel()) 
                {
                    channel.QueueBind(
                        queue: queueName,
                        exchange: BROKER_NAME, 
                        routingKey: eventName, 
                        arguments: null);
                }
            }
        }

        public void UnSubscribe<E, EH>()
            where E : IntegrationEvent
            where EH : IIntegrationEventHandler<E>
        {
            eventBusSubscribeManager.RemoveSubscribe<E, EH>();
        }
    }
}
