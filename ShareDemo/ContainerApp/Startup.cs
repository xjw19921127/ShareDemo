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
            //�������⴦��
            services.AddCorsSetup();

            //services.AddRzCoreServices(Configuration);
            //AppHub.ServiceProvider = services.BuildServiceProvider();

            //���APIȨ����֤����
            services.AddAuthSetup(Configuration);

            //Controller����
            services.AddControllerSetup();

            //���ݿ�����
            services.AddDatabaseSetup(Configuration);

            //Serviceע��
            services.AddDependencyInjectionSetup();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //ʹ��cors�������� 
            app.UseCors("cors");

            app.UseRouting();

            app.UseAuthentication();
            //ʹ��Ȩ����֤
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
