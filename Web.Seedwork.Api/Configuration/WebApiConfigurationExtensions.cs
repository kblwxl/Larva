using Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Seedwork.Api.Configuration
{
    public static class WebApiConfigurationExtensions
    {
        public static IWebApiModuleConfiguration WebApiModule(this IModuleConfigurations configurations)
        {
            return configurations.StartupConfiguration.GetOrCreate("WebSeedworkWebApiModuleConfiguration", () => configurations.StartupConfiguration.IocManager.Resolve<IWebApiModuleConfiguration>());
        }
    }
}
