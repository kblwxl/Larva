using Castle.Core;
using Castle.MicroKernel;
using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    internal static class CacheRegistrar
    {
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += ComponentRegistered;
        }
        private static void ComponentRegistered(string key, IHandler handler)
        {
            if (CacheHelper.IsConventionalApplicationClass(handler.ComponentModel.Implementation))
            {

                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(CacheInterceptor)));
            }
            else if (handler.ComponentModel.Implementation.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any(CacheHelper.HasCacheAttribute))
            {

                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(CacheInterceptor)));
            }
        }
    }
}
