using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Handlers
{
    public interface IEventHandler
    {

    }
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        
        void HandleEvent(TEventData eventData);
    }
}
