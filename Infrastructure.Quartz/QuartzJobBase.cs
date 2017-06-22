using Infrastructure.Dependency;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz
{
    public abstract class QuartzJobBase : IJob, ITransientDependency
    {
        public ILogger Logger { protected get; set; }
        public abstract void Execute(IJobExecutionContext context);
    }
}
