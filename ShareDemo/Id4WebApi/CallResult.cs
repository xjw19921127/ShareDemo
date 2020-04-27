using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi
{
    public class CallResult
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; }
    }

    public class CallResult<T> : CallResult
    {
        public T Data { get; set; }
    }
}
