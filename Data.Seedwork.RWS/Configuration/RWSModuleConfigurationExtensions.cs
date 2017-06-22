using Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration
{
    public static class RWSModuleConfigurationExtensions
    {
        public static IRWSModuleConfiguration RWSConfiguration(this IModuleConfigurations configuration)
        {
            return configuration.StartupConfiguration.GetOrCreate<IRWSModuleConfiguration>("RWSConfiguration", () => configuration.StartupConfiguration.IocManager.Resolve<IRWSModuleConfiguration>());
        }
    }
}
