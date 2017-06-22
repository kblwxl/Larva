using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules
{
    internal class ModuleInfo
    {
        public Assembly Assembly { get; private set; }
        public Type Type { get; private set; }
        public Module Instance { get; private set; }
        public List<ModuleInfo> Dependencies { get; private set; }
        public ModuleInfo(Module instance)
        {
            Dependencies = new List<ModuleInfo>();
            Type = instance.GetType();
            Instance = instance;
            Assembly = Type.Assembly;
        }
        public override string ToString()
        {
            return string.Format("{0}", Type.AssemblyQualifiedName);
        }
    }
}
