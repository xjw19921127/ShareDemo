using ContainerApp.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            //Service注入
            services.AddDependencyInjectionSetup();
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
        }
    }
}
