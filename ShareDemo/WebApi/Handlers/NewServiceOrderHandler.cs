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
    public class NewServiceOrderHandler : IRequestHandler<NewServiceOrderEvent,bool>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NewServiceOrderHandler> _logger;

        public NewServiceOrderHandler(IMediator mediator,ILogger<NewServiceOrderHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(NewServiceOrderEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Save ServiceOrder:{request.Message}");
            await _mediator.Publish(new AddServiceOrderSuccessEvent(request.Message));
            return true;
        }
    }
}
