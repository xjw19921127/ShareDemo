using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRedis
{
    public class DefaultRedisConnection : IRedisConnection
    {
        private string redisConnectString;
        private ConnectionMultiplexer connectionMultiplexer;
        private readonly ILogger<DefaultRedisConnection> logger;
        bool _disposed;
        object sync_root = new object();

        public DefaultRedisConnection(string redisConnectString, ILogger<DefaultRedisConnection> logger)
        {
            this.redisConnectString = redisConnectString;
            this.logger = logger;
        }

        public bool IsConnected { get { return connectionMultiplexer != null && connectionMultiplexer.IsConnected && !_disposed; } }

        public void Dispose()
        {
            if (_disposed) return;

            connectionMultiplexer.Dispose();
            _disposed = true;
        }

        public bool TryConnect()
        {
            lock (sync_root) 
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectString);
                if (IsConnected)
                {
                    connectionMultiplexer.ConnectionFailed += ConnectionMultiplexer_ConnectionFailed;
                    connectionMultiplexer.ConnectionRestored += ConnectionMultiplexer_ConnectionRestored;
                    connectionMultiplexer.ErrorMessage += ConnectionMultiplexer_ErrorMessage;
                    connectionMultiplexer.ConfigurationChanged += ConnectionMultiplexer_ConfigurationChanged;
                    connectionMultiplexer.HashSlotMoved += ConnectionMultiplexer_HashSlotMoved;
                    connectionMultiplexer.InternalError += ConnectionMultiplexer_InternalError;

                    logger.LogInformation("Redis connect success");

                    return true;
                }
                else 
                {
                    logger.LogCritical("Redis connection can not open");

                    return false;
                }
            }
        }

        private void ConnectionMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            if (_disposed) return;

            logger.LogWarning("Redis internal error, now reconnect...");

            TryConnect();
        }

        private void ConnectionMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            if (_disposed) return;

            logger.LogWarning("Redis hashslot is moved");
        }

        private void ConnectionMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            if (_disposed) return;

            logger.LogWarning("Redis configuration is changed, now reconnect...");

            TryConnect();
        }

        private void ConnectionMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            if (_disposed) return;

            logger.LogWarning("Redis connection is error, now reconnect...");

            TryConnect();
        }

        private void ConnectionMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            if (_disposed) return;

            logger.LogWarning("Redis connection is restored");
        }

        private void ConnectionMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            if (_disposed) return;

            logger.LogWarning("Redis connection is failed, now reconnect...");

            TryConnect();
        }

        public ConnectionMultiplexer GetInstance()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No Redis connection are available");
            }
            return connectionMultiplexer;
        }
    }
}
