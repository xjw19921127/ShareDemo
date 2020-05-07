using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAApi.Services
{
    public interface IUserService
    {
        public string GetUserName(string id);
        public Task<string> GetUserNameByMediatRAsync(string id);
    }
}
