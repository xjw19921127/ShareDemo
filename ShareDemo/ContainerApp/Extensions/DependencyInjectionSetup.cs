using ContainerApp.Event;
using ContainerApp.Handler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContainerApp.Extensions
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjectionSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddScoped<IRequestHandler<UserEvent, string>, UserHandler>();
        }
    }
}
