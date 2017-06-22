using Infrastructure.Caching;
using Infrastructure.Commands;
using Infrastructure.Configuration;
using Infrastructure.Dependency;
using Infrastructure.Events;
using Infrastructure.Modules;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class KernelModule : Modules.Module
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());
            CacheRegistrar.Initialize(IocManager);
            UnitOfWorkRegistrar.Initialize(IocManager);
            Configuration.UnitOfWork.RegisterFilter(DataFilters.SoftDelete, true);
        }
        public override void Initialize()
        {
            foreach (var replaceAction in ((StartupConfiguration)Configuration).ServiceReplaceActions.Values)
            {
                replaceAction();
            }
            IocManager.IocContainer.Install(
                new EventBusInstaller(IocManager),
                new CommandBusInstaller(IocManager)
                );
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(),
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
        }
        public override void PostInitialize()
        {
            RegisterMissingComponents();
        }
        private void RegisterMissingComponents()
        {
            IocManager.RegisterIfNot<IUnitOfWork, NullUnitOfWork>(DependencyLifeStyle.Transient);
            IocManager.RegisterIfNot<IDirectUnitOfWork, NullDirectUnitOfWork>(DependencyLifeStyle.Transient);
            IocManager.RegisterIfNot<ISlaveConnectionStringResolver, DefaultSlaveConnectionResolver>(DependencyLifeStyle.Transient);
        }
    }
}
