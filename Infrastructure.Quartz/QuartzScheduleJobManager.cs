using Infrastructure.Quartz.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz
{
    public class QuartzScheduleJobManager : IQuartzScheduleJobManager, Infrastructure.Dependency.ISingletonDependency
    {
        private IQuartzConfiguration quartzConfiguration;

        public QuartzScheduleJobManager(IQuartzConfiguration quartzConfiguration)
        {
            this.quartzConfiguration = quartzConfiguration;
        }
        public void Pause()
        {
            quartzConfiguration.Scheduler.PauseAll();
        }

        public void Resume()
        {
            quartzConfiguration.Scheduler.ResumeAll();
        }

        public void Start()
        {
            quartzConfiguration.Scheduler.Start();
        }

        public void Stop()
        {
            quartzConfiguration.Scheduler.Shutdown(false);
        }
    }
}
