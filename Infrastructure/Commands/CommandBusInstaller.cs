using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.Dependency;
using Infrastructure.Commands.Factories;

namespace Infrastructure.Commands
{
    internal class CommandBusInstaller : IWindsorInstaller
    {
        private readonly IIocResolver iocResolver;
        private CommandDispatcher commandDispatcher;

        public CommandBusInstaller(IIocResolver iocResolver)
        {
            this.iocResolver = iocResolver;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<CommandDispatcher>().Instance(new CommandDispatcher(iocResolver)).LifestyleSingleton()
                );
            commandDispatcher = iocResolver.Resolve<CommandDispatcher>();
            
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
        }

        private void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            if (!typeof(ICommandHandler).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                return;
            }

            var interfaces = handler.ComponentModel.Implementation.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(ICommandHandler).IsAssignableFrom(@interface))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    commandDispatcher.RegisteType(genericArgs[0], new IocCommandHandlerFactory(iocResolver, handler.ComponentModel.Implementation));
                }
            }
        }
    }
}
