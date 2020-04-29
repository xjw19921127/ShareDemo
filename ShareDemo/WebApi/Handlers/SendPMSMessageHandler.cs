using WebApi.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Handlers
{
    public class SendPMSMessageHandler : INotificationHandler<AddServiceOrderSuccessEvent>
    {
        private readonly ILogger<SendWXMessageHandler> _logger;

        public SendPMSMessageHandler(ILogger<SendWXMessageHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(AddServiceOrderSuccessEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Send PMS Message:{notification.Message}");
            return Task.CompletedTask;
        }
    }
}
