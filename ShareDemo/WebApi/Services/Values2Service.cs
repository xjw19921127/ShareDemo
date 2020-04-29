using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class Values2Service : IValuesService
    {
        private readonly ILogger<Values2Service> _logger;

        public Values2Service(ILogger<Values2Service> logger)
        {
            this._logger = logger;
        }

        public IEnumerable<string> FindAll()
        {
            this._logger.LogDebug("value2 {method} called", nameof(this.FindAll));

            return new[] { "value2" };
        }

        public string Find(int id)
        {
            this._logger.LogDebug("value2 {method} called with {id}", nameof(this.Find), id);

            return $"value2:{id}";
        }
    }
}
