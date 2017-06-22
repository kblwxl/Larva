using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Exceptions
{
    public class ExceptionData : EventData
    {
        
        public Exception Exception { get; private set; }

        
        public ExceptionData(Exception exception)
        {
            Exception = exception;
        }
    }
}
