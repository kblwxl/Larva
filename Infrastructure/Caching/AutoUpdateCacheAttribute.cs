using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    [AttributeUsageAttribute(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AutoUpdateCacheAttribute : CacheAttribute
    {
        public string Path { get; set; }
        public Type UpdaterType { get; private set; }

        public AutoUpdateCacheAttribute(string timeOut,Type updaterType)
            :this(timeOut,null,updaterType)
        {}
        public AutoUpdateCacheAttribute(string timeOut, string prefix,Type updaterType)
            :base(timeOut,prefix)
        {
            UpdaterType = updaterType;
        }
        public AutoUpdateCacheAttribute(CacheMethod method, Type updaterType)
            :this(method,null,updaterType)
        { }
        public AutoUpdateCacheAttribute(CacheMethod method,string prefix, Type updaterType)
            :base(method,prefix)
        {
            UpdaterType = updaterType;
        }
    }
}
