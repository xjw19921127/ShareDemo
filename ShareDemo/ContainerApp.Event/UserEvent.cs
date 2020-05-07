using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerApp.Event
{
    public class UserEvent : IRequest<string>
    {
        public string UserId { get; }

        public UserEvent(string id)
        {
            UserId = id;
        }
    }
}
