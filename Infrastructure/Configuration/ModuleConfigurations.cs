using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    internal class ModuleConfigurations : IModuleConfigurations
    {
        public IStartupConfiguration StartupConfiguration { get; private set; }
        public ModuleConfigurations(IStartupConfiguration configuration)
        {
            StartupConfiguration = configuration;
        }
    }
}
