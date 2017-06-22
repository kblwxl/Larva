using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands.Factories
{
    interface ICommandHandlerFactory
    {
        ICommandHandler GetHandler();
        void ReleaseHandler(ICommandHandler handler);
    }
}
