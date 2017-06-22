using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Seedwork.Configuration;

namespace Web.Seedwork
{
    [Infrastructure.Modules.DependsOn(
        typeof(Infrastructure.KernelModule))]
    public class WebSeedworkModule : Infrastructure.Modules.Module
    {
        public override void PreInitialize()
        {
            IocManager.Register<IWebModuleConfiguration, WebModuleConfiguration>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
