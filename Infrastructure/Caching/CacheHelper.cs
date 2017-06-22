using Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    internal static class CacheHelper
    {
        public static bool IsConventionalApplicationClass(Type targetType)
        {
            return typeof(IApplicationService).IsAssignableFrom(targetType);
        }
        public static bool HasCacheAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(CacheAttribute), true);
        }

    }
}
