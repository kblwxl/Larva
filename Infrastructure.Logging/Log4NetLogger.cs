using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging
{
    public class Log4NetLogger : ILogger
    {
        private log4net.ILog log;
        public Log4NetLogger()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
            log = log4net.LogManager.GetLogger(typeof(Log4NetLogger));
        }
        public void Debug(object item)
        {
            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            log = log4net.LogManager.GetLogger(mb.DeclaringType);
            
            log.Debug(item);
        }

        public void Debug(string message, params object[] args)
        {
            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            log = log4net.LogManager.GetLogger(mb.DeclaringType);
            log.DebugFormat(message, args);
        }

        public void Debug(string message, Exception exception, params object[] args)
        {
            log.DebugFormat(message + exception.Message + exception.StackTrace, args);
        }

        public void Error(string message, params object[] args)
        {
            log.ErrorFormat(message, args);
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            log.ErrorFormat(message + exception.Message + exception.StackTrace, args);
        }

        public void Fatal(string message, params object[] args)
        {
            log.FatalFormat(message, args);
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            log.Fatal(exception);
        }

        public void Info(string message, params object[] args)
        {
            log.InfoFormat(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            log.WarnFormat(message, args);
        }
    }
}
