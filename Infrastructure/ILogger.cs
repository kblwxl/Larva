using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ILogger
    {
        void Debug(string message, params object[] args);
        void Debug(string message, Exception exception, params object[] args);
        void Debug(object item);
        void Fatal(string message, params object[] args);
        void Fatal(string message, Exception exception, params object[] args);
        void Info(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(string message, params object[] args);
        void Error(string message, Exception exception, params object[] args);
    }
}
