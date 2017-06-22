using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Dependency;
using Infrastructure.Resource;

namespace Infrastructure.FileConverter
{
    public interface IFileConverterManager : Dependency.ISingletonDependency
    {
        List<IFileConverter> FileConverters { get;  }
        IResourceManager ResourceManager { get; }

    }
    
}
