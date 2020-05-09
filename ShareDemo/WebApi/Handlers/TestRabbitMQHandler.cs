using ContainerApp.Event;
using EventBus.Absratctions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Handlers
{
    public class TestRabbitMQHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        private readonly ILogger<TestRabbitMQHandler> logger;

        public TestRabbitMQHandler(ILogger<TestRabbitMQHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(TestIntegrationEvent @event)
        {
            logger.LogInformation($"TestRabbitMQHandler handle TestIntegrationEvent param:{JsonConvert.SerializeObject(@event)}");
            return Task.CompletedTask;
        }
    }
}
