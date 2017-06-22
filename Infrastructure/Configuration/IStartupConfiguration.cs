using Infrastructure.Dependency;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public interface IStartupConfiguration : IDictionaryBasedConfig
    {
        string DefaultNameOrConnectionString { get; set; }
        IIocManager IocManager { get; }
        IModuleConfigurations Modules { get; }
        IUnitOfWorkDefaultOptions UnitOfWork { get; }
        void ReplaceService(Type type, Action replaceAction);
    }
}
