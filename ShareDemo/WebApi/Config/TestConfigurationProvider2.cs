using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Config
{
    public class TestConfigurationProvider2 : ConfigurationProvider
    {
        public override void Load()
        {
            Data = new Dictionary<string, string> {
                { "xjw", "xujiawen2" }
            };
        }

        public override bool TryGet(string key, out string value)
        {
            if (key == "xjw")
            {
                value = "xujiawen22";
                return true;
            }
            return base.TryGet(key, out value);
        }
    }
}
