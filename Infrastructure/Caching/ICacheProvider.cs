using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public interface ICacheProvider : Infrastructure.Dependency.ISingletonDependency
    {
        void Put(string key, string valKey, object value,CacheAttribute cacheAttribute);
        object Get(string key, string valKey);
        void Remove(string key);
        void Remove(string key, string valueKey);
        void RemoveWithPrefix(string prefix);
        bool Exists(string key);
        bool Exists(string key, string valKey);
    }
}
