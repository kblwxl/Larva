using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration
{
    public interface IRWSModuleConfiguration
    {
        RWSConfigItem ConfigItem { get; }
    }
}
