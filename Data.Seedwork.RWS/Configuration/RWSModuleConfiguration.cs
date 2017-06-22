using Data.Seedwork.RWS.Configuration.ConfigSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS.Configuration
{
    public class RWSModuleConfiguration : IRWSModuleConfiguration
    {
        public RWSConfigItem ConfigItem { get; private set; }
        public RWSModuleConfiguration()
        {
            InitConfigProperties();
        }

        private void InitConfigProperties()
        {
            ConfigItem = new RWSConfigItem();
            var section = ReadOnlyDataBase.Instance;
            ConfigItem.HealthCheckIntervalSecond = section.HealthCheckIntervalSecond;
            ConfigItem.SlaveRandomization = section.SlaveRandomization;
            ConfigItem.SwitchMasterOnSlaveFailed = section.SwitchMasterOnSlaveFailed;
            ConfigItem.SwitchSlaveOnMasterFailed = section.SwitchSlaveOnMasterFailed;
            foreach(SlaveElement element in section.Slaves)
            {
                ConfigItem.Slaves.Add(new SlaveConfig { ConnectionStringName = element.ConnectionStringName, Offline = false });
            }
        }
    }
}
