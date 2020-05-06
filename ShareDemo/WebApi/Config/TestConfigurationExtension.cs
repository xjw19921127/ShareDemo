using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApi.Config.ConfigurationEnum;

namespace WebApi.Config
{
    public static class TestConfigurationExtension
    {
        public static IConfigurationBuilder AddTest(this IConfigurationBuilder configurationBuilder, ConfigurationSource configurationSourceEnum) 
        {
            if ((int)configurationSourceEnum == 1)
                return configurationBuilder.Add(new TestConfigurationSource());
            else if ((int)configurationSourceEnum == 2)
                return configurationBuilder.Add(new TestConfigurationSource2());
            else
                return configurationBuilder;
        }
    }
}
