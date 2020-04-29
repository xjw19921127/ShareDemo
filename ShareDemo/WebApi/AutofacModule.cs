using Autofac;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //使用RegisterType进行注册
            builder.RegisterType<Values2Service>();

            //通过类型注册为接口
            builder.RegisterType<ValuesService>().As<IValuesService>();

            //通过构造函数注册
            //builder.RegisterType<ValuesService>().UsingConstructor(typeof(ILogger<ValuesService>)).As<IValuesService>();

            //使用lambda表达式进行注册
            //builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
            //    .As<IValuesService>()
            //    .InstancePerLifetimeScope();

            //带属性赋值的注册
            builder.Register(c => new Values3Service() {
                //valuesService = c.Resolve<Values2Service>()
                valuesService = c.Resolve<IValuesService>()
            });

            //根据输入参数（NamedParameter）动态的选择实现类
            builder.Register<IValues2Service>((c, p) =>
            {
                var type = p.Named<string>("type");
                if (type == "4") return new Values4Service();
                else return new Values5Service();
            }).As<IValues2Service>();

            //注册单例
            builder.RegisterInstance(Values6Service.GetInstance()).ExternallyOwned();

            //注册泛型类
            builder.RegisterGeneric(typeof(CallResult<>));

            //用Name来区分不同的实现
            builder.RegisterType<Values4Service>().Named<IValues2Service>("4");
            builder.RegisterType<Values5Service>().Named<IValues2Service>("5");

            //注册Assembly下所有的类
            #region 使用属性注入的准备工作
            //Controller使用属性注入需要先把controller注入。如果通过new创建实例，该实例的属性无法使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(o => controllerBaseType.IsAssignableFrom(o) && o != controllerBaseType)
                .PropertiesAutowired();
            #endregion
        }
    }
}
