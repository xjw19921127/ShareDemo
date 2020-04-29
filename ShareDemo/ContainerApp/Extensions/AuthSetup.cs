using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace ContainerApp.Extensions
{
    public static class AuthSetup
    {
        public static void AddAuthSetup(this IServiceCollection services,IConfiguration configuration) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //var globalConfiguration = AppHub.GetService<IGlobalConfiguration>();
            //认证配置
            //services.AddRiiZooAuthSetup(configuration, globalConfiguration);
            //授权配置
            services.AddAuthorization(options =>
            {
                //角色校验
                options.AddPolicy("Admin", policy => policy.RequireRole(((int)RoleType.Admin).ToString()));
            });

            //显示认证错误信息
            //IdentityModelEventSource.ShowPII = true;
        }
    }

    public enum RoleType
    {
        [Description("管理员")]
        Admin = 1
    }
}
