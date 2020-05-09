using Autofac;
using ContainerApp.Event;
using ContainerApp.Extensions;
using ContainerApp.Handler;
using DemoAApi;
using DemoBApi;
using EventBus.Absratctions;
using EventBus.SubscribeManager;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ContainerApp
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
            //跨域问题处理
            services.AddCorsSetup();

            //services.AddRzCoreServices(Configuration);
            //AppHub.ServiceProvider = services.BuildServiceProvider();

            //添加API权限验证配置
            services.AddAuthSetup(Configuration);

            //Controller配置
            services.AddControllerSetup();

            //数据库配置
            services.AddDatabaseSetup(Configuration);
            
            //添加MediatR
            services.AddMediatR(typeof(Startup));

            //Service注入
            services.AddDependencyInjectionSetup();

            #region EventBus
            services.AddSingleton<IRabbitMQConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = Configuration["EventBusConnection"],
                        DispatchConsumersAsync = true
                    };

                    if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                        factory.UserName = Configuration["EventBusUserName"];

                    if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                        factory.Password = Configuration["EventBusPassword"];

                    return new DefaultRabbitMQConnection(factory, logger);
                });

            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = Configuration["SubscribeClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscribeManager>();

                return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, iLifetimeScope, logger, eventBusSubcriptionsManager, subscriptionClientName);
            });

            services.AddSingleton<IEventBusSubscribeManager, InMemoryEventBusSubscribeManager>();

            services.AddTransient<TestEventHandler>();
            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DemoAAutofacModule());
            builder.RegisterModule(new DemoBAutofacModule());
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //使用cors跨域配置 
            app.UseCors("cors");

            app.UseRouting();

            app.UseAuthentication();
            //使用权限验证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Home}/{action=Index}");
                endpoints.MapAreaControllerRoute(
                    name: "areas", "areas",
                    pattern: "api/{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<TestIntegrationEvent, TestEventHandler>();
        }
    }
}
