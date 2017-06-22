using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    [AttributeUsageAttribute(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CacheAttribute : Attribute
    {
        public static readonly TimeSpan DefaultTimeOut = TimeSpan.FromMinutes(30);
        public static readonly CacheStrategy DefaultStrategy = CacheStrategy.TimeOut;

        public CacheStrategy Strategy { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public CacheMethod Method { get; private set; }
        public string Prefix { get; private set; }

        
        public CacheAttribute()
        {
            Strategy = DefaultStrategy;
            Timeout = DefaultTimeOut;
        }
        public CacheAttribute(string timeout)
            :this(timeout,null)
        {
            
        }
        public CacheAttribute(TimeSpan timeout,string prefix)
        {
            Strategy = CacheStrategy.TimeOut;
            Timeout = timeout;
            Prefix = prefix;
        }
        public CacheAttribute(TimeSpan timeout)
            : this(timeout, null)
        {

        }
        public CacheAttribute(string timeout, string prefix)
        {
            Strategy = CacheStrategy.TimeOut;
            Timeout = TimeSpan.Parse(timeout);
            Prefix = prefix;
        }
        public CacheAttribute(CacheMethod method,string prefix)
        {
            Strategy = CacheStrategy.Manual;
            Method = method;
            Prefix = prefix;
        }
        public CacheAttribute(CacheMethod method)
            :this(method,null)
        {
            
        }

        public static CacheAttribute GetCacheAttribute(MethodInfo methodInfo)
        {
            var attr = methodInfo.GetCustomAttribute<CacheAttribute>();
            if(attr != null)
            {
                return attr;
            }
            return null;
        }
    }
}
