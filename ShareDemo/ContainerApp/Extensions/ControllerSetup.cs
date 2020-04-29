using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;

namespace ContainerApp.Extensions
{
    public static class ControllerSetup
    {
        public static void AddControllerSetup(this IServiceCollection services) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddControllers(options =>
            {
                //加入全局异常类
                options.Filters.Add<HttpGlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                //接口返回数据默认大小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //格式化时间
            });
        }
    }
}
