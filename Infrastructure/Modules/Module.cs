using Infrastructure.Configuration;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules
{
    public abstract class Module
    {
        protected internal IIocManager IocManager { get; internal set; }
        protected internal IStartupConfiguration Configuration { get; internal set; }
        public virtual void PreInitialize()
        {

        }
        public virtual void Initialize()
        {

        }
        public virtual void PostInitialize()
        {

        }
        public virtual void Shutdown()
        {

        }
        public static bool IsModule(Type type)
        {
            return
                type.IsClass &&
                !type.IsAbstract &&
                typeof(Module).IsAssignableFrom(type);
        }
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsModule(moduleType))
            {
                throw new Exception("此类型不是一个有效的模块: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetCustomAttributes(typeof(DependsOnAttribute), true).Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }

            return list;
        }
    }
}
