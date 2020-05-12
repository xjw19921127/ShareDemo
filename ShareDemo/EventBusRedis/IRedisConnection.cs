using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRedis
{
    public interface IRedisConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        ConnectionMultiplexer GetInstance();
    }
}
