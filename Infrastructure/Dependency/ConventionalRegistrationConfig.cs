using Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    public class ConventionalRegistrationConfig : DictionaryBasedConfig
    {
        public bool InstallInstallers { get; set; }
        public ConventionalRegistrationConfig()
        {
            InstallInstallers = true;
        }
    }
}
