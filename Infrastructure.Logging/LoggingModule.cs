using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configuration;

namespace Infrastructure.Logging
{
    [Infrastructure.Modules.DependsOn(typeof(Infrastructure.KernelModule))]
    public class LoggingModule : Infrastructure.Modules.Module
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService<ILogger, Log4NetLogger>();
        }
    }
}
