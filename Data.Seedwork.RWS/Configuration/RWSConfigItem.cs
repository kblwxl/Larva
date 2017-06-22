using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration
{
    /// <summary>
    /// 读写分离的配置信息
    /// </summary>
    /// <remarks>
    /// 此配置类中包含的信息应从配置文件中读取。
    /// </remarks>
    public class RWSConfigItem
    {
        public bool SwitchSlaveOnMasterFailed { get; internal set; }
        public bool SwitchMasterOnSlaveFailed { get; internal set; }
        public int HealthCheckIntervalSecond { get; internal set; }
        public bool SlaveRandomization { get; internal set; }
        public List<SlaveConfig> Slaves { get; internal set; }

        public RWSConfigItem()
        {
            SwitchMasterOnSlaveFailed = true;
            SwitchSlaveOnMasterFailed = false;
            HealthCheckIntervalSecond = 30;
            SlaveRandomization = true;
            Slaves = new List<SlaveConfig>();
        }
    }
}
