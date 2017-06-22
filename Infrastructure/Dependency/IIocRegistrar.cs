using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    public interface IIocRegistrar
    {
        void AddConventionalRegistrar(IConventionalDependencyRegistrar registrar);
        void RegisterAssemblyByConvention(Assembly assembly);
        void RegisterAssemblyByConvention(Assembly assembly, ConventionalRegistrationConfig config);
        void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where T : class;
        void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);
        void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton)
            where TType : class
            where TImpl : class, TType;
        void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton);
        bool IsRegistered(Type type);
        bool IsRegistered<TType>();
    }
}
