using Data.Seedwork.EF;
using Data.Seedwork.RWS.Configuration;
using Infrastructure.Collections.Extensions;
using Infrastructure.Modules;
using Infrastructure.Reflection;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS
{
    [DependsOn(typeof(Infrastructure.KernelModule),typeof(Data.Seedwork.EF.DataSeedworkEntityFrameworkModule))]
    public class RWSModule : Module
    {
        public override void PreInitialize()
        {
            IocManager.Register<IRWSModuleConfiguration, RWSModuleConfiguration>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(System.Reflection.Assembly.GetExecutingAssembly());
            IocManager.Register<ISlaveConnectionStringResolver, SlaveConnectionStringResolver>(Infrastructure.Dependency.DependencyLifeStyle.Transient);
            IocManager.Register<IDirectUnitOfWork, EFDirectUnitOfWork>(Infrastructure.Dependency.DependencyLifeStyle.Transient);
        }
        
    }
}
