﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Infrastructure.Quartz.Configuration
{
    public class QuartzConfiguration : IQuartzConfiguration
    {
        public IScheduler Scheduler
        {
            get
            {
                return StdSchedulerFactory.GetDefaultScheduler();
            }
        }
    }
}
