using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Id4WebApi.Services
{
    public class Values6Service
    {
        private static readonly Lazy<Values6Service> _singleton = new Lazy<Values6Service>(() => new Values6Service());

        public static Values6Service GetInstance() => _singleton.Value;

        public IEnumerable<string> FindAll()
        {
            return new[] { "value6" };
        }

        public string Find(int id)
        {
            return $"value6:{id}";
        }
    }
}
