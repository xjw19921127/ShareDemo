using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WebApi.Services
{
    public class Values3Service : IValuesService
    {
        public IValuesService valuesService { get; set; }

        public IEnumerable<string> FindAll()
        {
            return valuesService.FindAll();
        }

        public string Find(int id)
        {
            return valuesService.Find(id);
        }
    }
}
