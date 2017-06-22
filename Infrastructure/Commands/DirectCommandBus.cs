using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class DirectCommandBus : ICommandBus
    {
        public ILogger Logger { get; set; }
        private CommandDispatcher dispatcher;
        public DirectCommandBus(Dependency.IIocResolver iocResolver)
        {
            dispatcher = iocResolver.Resolve<CommandDispatcher>();
            
        }

        
        public virtual void Send(ICommand command)
        {
            if (command == null)
            {
                ArgumentNullException error = new ArgumentNullException(nameof(command));
                Logger.Error(error.Message, error);
                throw error;
            }
            dispatcher.DispatchCommand(command);
        }
    }
}
