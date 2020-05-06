using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Validators;

namespace WebApi
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
            #region ��֤
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:50024";
                    options.ApiName = "WebApi";
                    options.EnableCaching = false;
                    options.ApiSecret = "654321";
                    options.RequireHttpsMetadata = false;
                    options.OAuth2IntrospectionEvents = new OAuth2IntrospectionEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // ������ʣ�401����Ȩ��֤ʧ��
                            var tokenType = context.Options.TokenTypeHint;
                            if (context.Error.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            context.Response.ContentType = "application/json";
                            //�Զ��巵��״̬�룬Ĭ��Ϊ401
                            //context.Response.StatusCode = StatusCodes.Status200OK;
                            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.WriteAsync(JsonConvert.SerializeObject(new { code = "401", msg = "Unauthorized" }));
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            // ��������
                            var token = context.SecurityToken;
                            return Task.CompletedTask;
                        },
                    };
                });
            #endregion

            services.AddControllers().AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<StoreAddDTOVaildator>();
                cfg.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            }).AddControllersAsServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(typeof(Startup));

            services.Configure<ApiBehaviorOptions>(opt => 
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(o => o.Value.Errors.Count > 0).Select(o => o.Value.Errors.First().ErrorMessage).ToList();
                    var msg = string.Join(" && ", errors);
                    return new JsonResult(new CallResult{ 
                        StatusCode = "500",
                        Message = msg
                    });
                };
            });

            var xjwName = Configuration["xjw"];
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ILifetimeScope autofacRoot = app.ApplicationServices.GetAutofacRoot();
            ServiceLocator.SetContainer(autofacRoot);
        }
    }
}
