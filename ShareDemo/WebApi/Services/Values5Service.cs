using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class Values5Service : IValues2Service
    {
        public IEnumerable<string> FindAll()
        {
            return new[] { "value5" };
        }

        public string Find(int id)
        {
            return $"value5:{id}";
        }
    }
}
