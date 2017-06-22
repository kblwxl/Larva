using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules
{
    internal interface IModuleManager
    {
        void InitializeModules();

        void ShutdownModules();
    }
}
