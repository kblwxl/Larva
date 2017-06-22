using Infrastructure.Configuration;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        private readonly IStartupConfiguration _configuration;
        public DefaultConnectionStringResolver(IStartupConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual string GetNameOrConnectionString()
        {
            
            var defaultConnectionString = _configuration.DefaultNameOrConnectionString;
            if (!string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }

            throw new Exception("无法找到数据库连接字符串. 请设置 IStartupConfiguration.DefaultNameOrConnectionString 属性或者在应用程序配置文件中添加键为Default的数据库连接字符串.");
        }
    }
}
