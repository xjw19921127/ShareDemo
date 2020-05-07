using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBApi.Services
{
    public interface IUserService
    {
        public string GetUserName(string id);
    }
}
