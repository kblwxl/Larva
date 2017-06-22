using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    /// <summary>
    /// 实习此接口的类会自动在IOC容器中注册，并且其生命周期为‘单例’
    /// </summary>
    public interface ISingletonDependency
    {
    }
}
