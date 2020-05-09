using ContainerApp.Event;
using EventBus.Absratctions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContainerApp.Handler
{
    public class TestEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        private readonly ILogger<TestEventHandler> logger;

        public TestEventHandler(ILogger<TestEventHandler> logger)
        {
            this.logger = logger;
        }
        public Task Handle(TestIntegrationEvent @event)
        {
            logger.LogInformation($"TestEventHandler Handle TestIntegrationEvent param:{JsonConvert.SerializeObject(@event)}");
            return Task.CompletedTask;
        }
    }
}
