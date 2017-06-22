using Application.Seedwork.Interceptors;
using Infrastructure.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seedwork
{
    [DependsOn(typeof(Infrastructure.KernelModule))]
    public class ApplicationSeedworkModule : Infrastructure.Modules.Module
    {
        public override void PreInitialize()
        {
            ValidationInterceptorRegistrar.Initialize(IocManager);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
