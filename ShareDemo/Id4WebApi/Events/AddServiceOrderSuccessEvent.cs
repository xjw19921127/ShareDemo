using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi.Events
{
    public class AddServiceOrderSuccessEvent : INotification
    {
        public string Message { get; }

        public AddServiceOrderSuccessEvent(string message)
        {
            Message = message;
        }
    }
}
