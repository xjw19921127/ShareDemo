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
                { "xjw", "xujiawen1" }
            };
        }

        public override bool TryGet(string key, out string value)
        {
            if (key == "xjw")
            {
                value = "xujiawen11";
                return true;
            }
            return base.TryGet(key, out value);
        }
    }
}
