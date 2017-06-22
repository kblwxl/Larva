using Infrastructure;
using Infrastructure.Dependency;
using Infrastructure.Events;
using Infrastructure.Events.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Web.Seedwork.Api.Filters
{
    public class LoggingExceptionFilterAttribute : ExceptionFilterAttribute, ITransientDependency
    {
        public ILogger Logger { get; set; }
        public IEventBus EventBus { get; set; }
        public LoggingExceptionFilterAttribute()
        {
            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.Error(string.Empty, context.Exception);
            EventBus.Trigger(this, new HandledExceptionData(context.Exception));
        }
    }
}
