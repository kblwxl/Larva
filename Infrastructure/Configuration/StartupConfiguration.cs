using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dependency;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Configuration
{
    internal class StartupConfiguration : DictionaryBasedConfig, IStartupConfiguration
    {
        public string DefaultNameOrConnectionString { get;  set; }

        public IIocManager IocManager { get; private set; }
        public IEventBusConfiguration EventBus { get; private set; }

        public IModuleConfigurations Modules { get; private set; }

        public IUnitOfWorkDefaultOptions UnitOfWork { get; private set; }

        public Dictionary<Type, Action> ServiceReplaceActions { get; private set; }

        
        public StartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }
        public void Initialize()
        {
            Modules = IocManager.Resolve<IModuleConfigurations>();
            UnitOfWork = IocManager.Resolve<IUnitOfWorkDefaultOptions>();
            EventBus = IocManager.Resolve<IEventBusConfiguration>();
            ServiceReplaceActions = new Dictionary<Type, Action>();
        }

        public void ReplaceService(Type type, Action replaceAction)
        {
            ServiceReplaceActions[type] = replaceAction;
        }
    }
}
