using ContainerApp.Event;
using EventBus.Absratctions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoAApi.Handlers
{
    public class TestEventHandler2 : IIntegrationEventHandler<TestIntegrationEvent>
    {
        private readonly ILogger<TestEventHandler2> logger;

        public TestEventHandler2(ILogger<TestEventHandler2> logger)
        {
            this.logger = logger;
        }
        public Task Handle(TestIntegrationEvent @event)
        {
            logger.LogInformation($"TestEventHandler2 Handle TestIntegrationEvent param:{JsonConvert.SerializeObject(@event)}");
            return Task.CompletedTask;
        }
    }
}
