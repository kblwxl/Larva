using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Web.Seedwork.Api.Configuration
{
    public interface IWebApiModuleConfiguration
    {
        HttpConfiguration HttpConfiguration { get; set; }
    }
}
