using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var api = new IdentityServer4.Models.ApiResource("WebApi", "WebApi", new List<string>{ "hotel",
                "store",
                "user",
                "role",
                "other" });

            api.ApiSecrets = new List<Secret> {
                new IdentityServer4.Models.Secret("654321".Sha256())
            };

            var apis = new List<IdentityServer4.Models.ApiResource> {
                api
            };
            var clients = new List<IdentityServer4.Models.Client> { 
                new IdentityServer4.Models.Client{
                    ClientId = "hms_client",
                    AllowedGrantTypes = new List<string>{ "password" },
                    AllowOfflineAccess = true,
                    AccessTokenType = AccessTokenType.Reference,
                    AccessTokenLifetime = 86400,
                    SlidingRefreshTokenLifetime = 1296000,
                    ClientSecrets = { new IdentityServer4.Models.Secret("123456".Sha256()) },
                    AllowedScopes = new List<string>{ "WebApi" }
                },
            };

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(apis)
                .AddInMemoryClients(clients)
                .AddInMemoryIdentityResources(new IdentityServer4.Models.IdentityResource[]
                {
                    new IdentityServer4.Models.IdentityResources.OpenId(),
                    new IdentityServer4.Models.IdentityResources.Profile()
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = context =>
                    {
                        context.UseMySQL(Configuration.GetConnectionString("AuthDB"), sql =>
                        {
                            sql.MigrationsAssembly(migrationsAssembly);
                        });
                    };
                    opt.EnableTokenCleanup = true;
                    opt.TokenCleanupInterval = 30;
                })
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
