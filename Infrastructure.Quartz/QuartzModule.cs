using Infrastructure.Modules;
using Infrastructure.Quartz.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz
{
    [DependsOn(typeof(KernelModule))]
    public class QuartzModule : Module
    {
        public override void PreInitialize()
        {
            IocManager.Register<IQuartzConfiguration, QuartzConfiguration>();
            Configuration.Modules.Quartz().Scheduler.JobFactory = new QuartzJobFactory(IocManager);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
}
