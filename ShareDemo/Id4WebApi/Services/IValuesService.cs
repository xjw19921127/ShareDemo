using System.Collections.Generic;

namespace Id4WebApi.Services
{
    public interface IValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
}