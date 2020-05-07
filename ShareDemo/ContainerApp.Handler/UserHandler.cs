using ContainerApp.Event;
using DemoBApi.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerApp.Handler
{
    public class UserHandler : IRequestHandler<UserEvent, string>
    {
        private readonly IUserService userService;

        public UserHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<string> Handle(UserEvent request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
                return userService.GetUserName(request.UserId);
            return "";
        }
    }
}
