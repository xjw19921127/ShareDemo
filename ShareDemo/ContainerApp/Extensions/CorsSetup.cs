using Microsoft.Extensions.DependencyInjection;
using System;

namespace ContainerApp.Extensions
{
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
        }
    }
}
