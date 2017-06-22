using Infrastructure.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Factories.Internals
{
    internal class TransientEventHandlerFactory<THandler> : IEventHandlerFactory
        where THandler : IEventHandler, new()
    {
        
        public IEventHandler GetHandler()
        {
            return new THandler();
        }

        
        public void ReleaseHandler(IEventHandler handler)
        {
            if (handler is IDisposable)
            {
                (handler as IDisposable).Dispose();
            }
        }
    }
}
