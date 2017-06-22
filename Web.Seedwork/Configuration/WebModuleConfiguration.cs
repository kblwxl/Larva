using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Seedwork.Configuration
{
    internal class WebModuleConfiguration : IWebModuleConfiguration
    {
        public bool SendAllExceptionsToClients { get; set; }
    }
}
