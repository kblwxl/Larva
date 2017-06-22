using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class NullLogger : ILogger
    {
        private static readonly NullLogger instance = new NullLogger();
        public static NullLogger Instance
        {
            get { return instance; }
        }
        public void Debug(object item)
        {
            
        }

        public void Debug(string message, params object[] args)
        {
            
        }

        public void Debug(string message, Exception exception, params object[] args)
        {
            
        }

        public void Fatal(string message, params object[] args)
        {
            
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            
        }

        public void Error(string message, params object[] args)
        {
            
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            
        }

        public void Info(string message, params object[] args)
        {
            
        }

        public void Warning(string message, params object[] args)
        {
            
        }
    }
}
