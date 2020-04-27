using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi.Events
{
    public class NewServiceOrderEvent : IRequest<bool>
    {
        public string Message { get; }

        public NewServiceOrderEvent(string message)
        {
            Message = message;
        }
    }
}
