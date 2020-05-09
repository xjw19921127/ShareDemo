using Autofac;
using ContainerApp.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAApi.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IMediator mediator;

        public UserServiceImpl(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public string GetUserName(string id)
        {
            return "xjw-A";
        }

        public async Task<string> GetUserNameByMediatRAsync(string id)
        {
            return await mediator.Send(new UserEvent(id));
        }
    }
}
