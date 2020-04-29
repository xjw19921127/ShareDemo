using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class ValuesService : IValuesService
    {
        private readonly ILogger<ValuesService> _logger;

        public ValuesService(ILogger<ValuesService> logger)
        {
            this._logger = logger;
        }

        public IEnumerable<string> FindAll()
        {
            this._logger.LogDebug("value1 {method} called", nameof(this.FindAll));

            return new[] { "value1" };
        }

        public string Find(int id)
        {
            this._logger.LogDebug("value1 {method} called with {id}", nameof(this.Find), id);

            return $"value1:{id}";
        }
    }
}
