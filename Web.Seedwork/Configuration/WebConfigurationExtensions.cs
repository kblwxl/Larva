using Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Seedwork.Configuration
{
    public static class WebConfigurationExtensions
    {
        public static IWebModuleConfiguration WebConfiguration(this IModuleConfigurations configurations)
        {
            return configurations.StartupConfiguration.GetOrCreate("WebSeedworkModuleConfiguation", () => configurations.StartupConfiguration.IocManager.Resolve<IWebModuleConfiguration>());
        }
    }
}
