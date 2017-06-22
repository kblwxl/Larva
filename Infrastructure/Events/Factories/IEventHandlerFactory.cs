using Infrastructure.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Factories
{
    public interface IEventHandlerFactory
    {
        IEventHandler GetHandler();
        void ReleaseHandler(IEventHandler handler);
    }
}
