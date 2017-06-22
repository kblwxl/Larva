using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public interface ICommandHandler
    {
    }
    public interface ICommandHandler<in TCommand>:ICommandHandler
        where TCommand:ICommand
    {
        void Handler(TCommand command);
    }
}
