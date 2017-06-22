using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class DefaultSlaveConnectionResolver : ISlaveConnectionStringResolver
    {
        private IConnectionStringResolver masterConnectionStringResolver;
        public DefaultSlaveConnectionResolver(IConnectionStringResolver connectionStringResolver)
        {
            masterConnectionStringResolver = connectionStringResolver;
        }
        public string GetNameOrConnectionString()
        {
            return masterConnectionStringResolver.GetNameOrConnectionString();
        }
    }
}
