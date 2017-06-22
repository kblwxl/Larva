using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Infrastructure.Dependency;

namespace Infrastructure.Quartz
{
    public class QuartzJobFactory : IJobFactory
    {
        private IIocResolver iocResolver;
        public QuartzJobFactory(IIocResolver iocResolver)
        {
            this.iocResolver = iocResolver;
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return iocResolver.Resolve(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            iocResolver.Release(job);
        }
    }
}
