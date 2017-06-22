using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.UnitOfWork;
using Infrastructure.Configuration;
using Infrastructure.Reflection;
using Infrastructure.Modules;

namespace Infrastructure.Dependency.Installers
{
    internal class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                 Component.For<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>().ImplementedBy<UnitOfWorkDefaultOptions>().LifestyleSingleton(),
                 Component.For<IModuleConfigurations, ModuleConfigurations>().ImplementedBy<ModuleConfigurations>().LifestyleSingleton(),
                 Component.For<IStartupConfiguration, StartupConfiguration>().ImplementedBy<StartupConfiguration>().LifestyleSingleton(),
                 Component.For<IEventBusConfiguration, EventBusConfiguration>().ImplementedBy<EventBusConfiguration>().LifestyleSingleton(),
                 Component.For<ITypeFinder>().ImplementedBy<TypeFinder>().LifestyleSingleton(),
                 Component.For<IModuleFinder>().ImplementedBy<DefaultModuleFinder>().LifestyleTransient(),
                 Component.For<IModuleManager>().ImplementedBy<ModuleManager>().LifestyleSingleton()
                );
        }
    }
}
