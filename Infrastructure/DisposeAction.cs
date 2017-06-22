using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;
        public DisposeAction(Action action)
        {
            if(action==null)
            {
                throw new ArgumentNullException("DisposeAction");
            }
            _action = action;
        }
        public void Dispose()
        {
            _action();
        }
    }
}
