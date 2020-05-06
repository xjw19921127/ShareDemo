using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Config
{
    public class TestConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            Data = new Dictionary<string, string> {
                { "xjw", "xujiawen" }
            };
        }
    }
}
