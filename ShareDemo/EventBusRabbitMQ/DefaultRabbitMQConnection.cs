using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    public class DefaultRabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly ILogger<DefaultRabbitMQConnection> logger;
        bool _disposed;
        IConnection connection;
        object sync_root = new object();

        public DefaultRabbitMQConnection(IConnectionFactory connectionFactory,ILogger<DefaultRabbitMQConnection> logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public bool IsConnected { get { return connection != null && connection.IsOpen && !_disposed; } }

        public IModel CreateModel()
        {
            if (!IsConnected) 
            {
                throw new InvalidOperationException("No RabbitMQ connection are available");
            }
            return connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            connection.Dispose();
            _disposed = true;
        }

        public bool TryConnect()
        {
            lock (sync_root) 
            {
                connection = connectionFactory.CreateConnection();
                if (IsConnected)
                {
                    connection.ConnectionShutdown += OnConnectionShutdown;
                    connection.ConnectionBlocked += OnConnectionBlocked;
                    connection.CallbackException += OnCallbackException;

                    logger.LogInformation("RabbitMQ connect to {HostName}", connection.Endpoint.HostName);

                    return true;
                }
                else 
                {
                    logger.LogCritical("RabbitMQ connection can not open");

                    return false;
                }
            }
        }

        private void OnConnectionShutdown(object sender,ShutdownEventArgs args) 
        {
            if (_disposed) return;

            logger.LogWarning("RabbitMQ Connection is shutdown, now reconnect...");

            TryConnect();
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs args) 
        {
            if (_disposed) return;

            logger.LogWarning("RabbitMQ Connection is blocked, now reconnect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs args) 
        {
            if (_disposed) return;

            logger.LogWarning("RabbitMQ Connect is callback exception, now reconnect...");

            TryConnect();
        }
    }
}
