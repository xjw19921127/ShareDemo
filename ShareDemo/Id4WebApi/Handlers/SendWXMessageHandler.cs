using Id4WebApi.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Id4WebApi.Handlers
{
    public class SendWXMessageHandler : INotificationHandler<AddServiceOrderSuccessEvent>
    {
        private readonly ILogger<SendWXMessageHandler> _logger;

        public SendWXMessageHandler(ILogger<SendWXMessageHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(AddServiceOrderSuccessEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Send WX Message:{notification.Message}");
            return Task.CompletedTask;
        }
    }
}
