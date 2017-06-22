using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    public interface IConventionalRegistrationContext
    {
        Assembly Assembly { get; }
        IIocManager IocManager { get; }
        ConventionalRegistrationConfig Config { get; }
    }
}
