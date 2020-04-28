using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Id4WebApi.Services
{
    public class Values4Service : IValues2Service
    {
        public IEnumerable<string> FindAll()
        {
            return new[] { "value4" };
        }

        public string Find(int id)
        {
            return $"value4:{id}";
        }
    }
}
