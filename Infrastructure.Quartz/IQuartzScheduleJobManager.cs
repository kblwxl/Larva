using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quartz
{
    public interface IQuartzScheduleJobManager
    {
        void Start();
        void Stop();
        void Pause();
        void Resume();
    }
}
