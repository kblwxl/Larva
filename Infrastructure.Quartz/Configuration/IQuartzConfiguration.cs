using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz.Configuration
{
    public interface IQuartzConfiguration
    {
        IScheduler Scheduler { get; }
    }
}
