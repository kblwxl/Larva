using Data.Seedwork.RWS.Configuration;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.RWS
{
    public class SlaveConnectionStringResolver : ISlaveConnectionStringResolver
    {
        private IRWSModuleConfiguration config;
        private IConnectionStringResolver defaultConnectionStringResolver;
        public SlaveConnectionStringResolver(
            IRWSModuleConfiguration config,
            IConnectionStringResolver defaultConnectionStringResolver)
        {
            this.config = config;
            this.defaultConnectionStringResolver = defaultConnectionStringResolver;
        }
        public string GetNameOrConnectionString()
        {
            var slaveConfigs = config.ConfigItem.Slaves.Where(p => p.Offline == false).ToList();
            if(slaveConfigs==null || slaveConfigs.Count<=0)
            {
                return defaultConnectionStringResolver.GetNameOrConnectionString();
            }
            if(slaveConfigs.Count==1 && !config.ConfigItem.SlaveRandomization)
            {
                return slaveConfigs[0].ConnectionStringName;
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return slaveConfigs[random.Next(slaveConfigs.Count)].ConnectionStringName;
        }
    }
}
