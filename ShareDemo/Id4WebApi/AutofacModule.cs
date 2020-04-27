using Autofac;
using Id4WebApi.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ////----------------------反射组件----------------////
            //通过类型注册
            //builder.RegisterType<ValuesService>().As<IValuesService>();
            //通过构造函数注册
            //builder.RegisterType<ValuesService>().UsingConstructor(typeof(ILogger<ValuesService>)).As<IValuesService>();

            ////------------------Lambda表达式组件------------////
            builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
                .As<IValuesService>()
                .InstancePerLifetimeScope();

            //分离组件创建最大的好处就是具体类型可以变化
            //一个组件分离的Demo:注册一个信用卡组件
            //builder.Register<CreditCard>(
            //    (c, p) =>
            //    {
            //        var accountId = p.Named<string>("accountId");
            //        if (accountId.StartsWith("9"))
            //        {
            //            return new GoldCard(accountId);
            //        }
            //        else
            //        {
            //            return new StandardCard(accountId);
            //        }
            //    });
            //根据参数不同获取不同种类的信用卡类实例
            //var card = container.Resolve<CreditCard>(new NamedParameter("accountId", "12345"));
        }
    }
}
