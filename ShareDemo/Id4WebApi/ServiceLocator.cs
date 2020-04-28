using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi
{
    public class ServiceLocator
    {
        public static ILifetimeScope _lifetimeScope;
        public static void SetContainer(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }
    }
}
