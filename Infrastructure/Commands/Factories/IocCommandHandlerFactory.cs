using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands.Factories
{
    class IocCommandHandlerFactory : ICommandHandlerFactory
    {
        public Type HandlerType { get; private set; }
        private readonly Dependency.IIocResolver iocResolver;

        public IocCommandHandlerFactory(Dependency.IIocResolver iocResolver,Type handlerType)
        {
            this.iocResolver = iocResolver;
            HandlerType = handlerType;
        }
        public ICommandHandler GetHandler()
        {
            return iocResolver.Resolve(HandlerType) as ICommandHandler;
        }

        public void ReleaseHandler(ICommandHandler handler)
        {
            iocResolver.Release(handler);
        }
    }
}
