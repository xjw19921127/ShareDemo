using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi
{
    public class ServiceOrderDTO
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public InvoiceTitleDTO TitleInfo {get;set;}
    }
}
