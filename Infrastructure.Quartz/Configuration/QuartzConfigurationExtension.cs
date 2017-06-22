using Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz.Configuration
{
    public static class QuartzConfigurationExtension
    {
        public static IQuartzConfiguration Quartz(this IModuleConfigurations configurations)
        {
            return configurations.StartupConfiguration.GetOrCreate("QuartzModuleConfiguration", () => configurations.StartupConfiguration.IocManager.Resolve<IQuartzConfiguration>());
        }
    }
}
