using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Seedwork.Configuration
{
    public interface IWebModuleConfiguration
    {
        /// <summary>
        /// 是否将异常信息直接发送到客户端，默认为false
        /// </summary>
        bool SendAllExceptionsToClients { get; set; }
    }
}
