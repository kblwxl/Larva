using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
    public interface IIocResolver
    {
        T Resolve<T>();
        T Resolve<T>(Type type);
        T Resolve<T>(object argumentsAsAnonymousType);
        List<T> ResolveAll<T>();
        object Resolve(Type type);
        object Resolve(Type type, object argumentsAsAnonymousType);
        void Release(object obj);
        bool IsRegistered(Type type);
        bool IsRegistered<T>();
    }
}
