using System.Collections.Generic;

namespace Id4WebApi.Services
{
    public interface IValues2Service
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
}