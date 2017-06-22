using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public class CacheInterceptor : IInterceptor
    {
        public ICacheProvider cacheProvider { get; set; }
        public Dependency.IIocResolver iocResolver { get; set; }
        private string GetValueKey(IInvocation invocation,CacheAttribute cacheAttribute)
        {
            string retValue = null;
            switch(cacheAttribute.Strategy)
            {
                case CacheStrategy.TimeOut:
                    retValue = GenValueKey(invocation);
                    break;
                case CacheStrategy.Manual:
                    switch(cacheAttribute.Method)
                    {
                        case CacheMethod.Get:
                        case CacheMethod.Put:
                            retValue = GenValueKey(invocation);
                            break;
                    }
                    break;
            }
            return retValue;
        }

        private string GenValueKey(IInvocation invocation)
        {
            string retValue = "NULL";
            var sb = new StringBuilder(); 
            if (invocation.Arguments.Length > 0)
            {
                
                for(int i=0;i<invocation.Arguments.Length;i++)
                {
                    if(null != invocation.Arguments[i])
                    {
                        sb.Append(invocation.Arguments[i].ToString());
                        if (i != invocation.Arguments.Length - 1)
                            sb.Append("_");
                    }
                    
                }
                retValue = sb.ToString(); 
            }
            return retValue;
        }

        public void Intercept(IInvocation invocation)
        {
            CacheAttribute cacheAttribute = CacheAttribute.GetCacheAttribute(invocation.Method);
            bool isVoidReturnValue = invocation.Method.ReturnParameter.ParameterType.Equals(typeof(void));
            if (cacheAttribute != null)
            {
                string key = string.Format("{0}{1}.{2}", cacheAttribute.Prefix, invocation.TargetType.FullName,invocation.Method.Name);
                string valKey = GetValueKey(invocation, cacheAttribute);
                switch(cacheAttribute.Strategy)
                {
                    case CacheStrategy.TimeOut:
                        if(cacheProvider.Exists(key,valKey))
                        {
                            if(!isVoidReturnValue)
                            {
                                SetRetValueFromCache(invocation, cacheAttribute, key, valKey);
                            }

                            return;
                        }
                        else
                        {
                            invocation.Proceed();
                            if (!isVoidReturnValue)
                                cacheProvider.Put(key, valKey, invocation.ReturnValue, cacheAttribute);
                        }
                        break;
                    case CacheStrategy.Manual:
                        switch(cacheAttribute.Method)
                        {
                            case CacheMethod.Get:
                                if (cacheProvider.Exists(key, valKey))
                                {
                                    if (!isVoidReturnValue)
                                        SetRetValueFromCache(invocation, cacheAttribute, key, valKey);
                                    return;
                                }
                                else
                                {
                                    invocation.Proceed();
                                    if (!isVoidReturnValue)
                                        cacheProvider.Put(key, valKey, invocation.ReturnValue, cacheAttribute);
                                }
                                break;
                            case CacheMethod.Put:
                                invocation.Proceed();
                                if (!isVoidReturnValue)
                                    cacheProvider.Put(key, valKey, invocation.ReturnValue, cacheAttribute);
                                break;
                            case CacheMethod.Remove:
                                if (string.IsNullOrEmpty(cacheAttribute.Prefix))
                                    cacheProvider.Remove(key);
                                else
                                    cacheProvider.RemoveWithPrefix(cacheAttribute.Prefix);
                                invocation.Proceed();
                                break;
                        }
                        break;
                }
            }
            else
            {
                invocation.Proceed();
            }
            
        }

        private void SetRetValueFromCache(IInvocation invocation, CacheAttribute cacheAttribute, string key, string valKey)
        {
            object cachedItem = cacheProvider.Get(key, valKey);
            if (cacheAttribute is AutoUpdateCacheAttribute)
            {
                AutoUpdateCacheAttribute autoUpdateAttribute = (AutoUpdateCacheAttribute)cacheAttribute;
                object waitUpdateItem = cachedItem;
                if (!string.IsNullOrEmpty(autoUpdateAttribute.Path))
                {
                    waitUpdateItem = cachedItem.GetType().GetProperty(autoUpdateAttribute.Path).GetValue(cachedItem);
                }
                Type itemType = waitUpdateItem.GetType();
                bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(waitUpdateItem.GetType());
                if (isEnumerable)
                {
                    itemType = waitUpdateItem.GetType().GetGenericArguments()[0];
                }
                //var updater = Activator.CreateInstance(autoUpdateAttribute.UpdaterType);
                var updater = this.iocResolver.Resolve(autoUpdateAttribute.UpdaterType);
                var updaterType = typeof(ICachedItemUpdater<>).MakeGenericType(itemType);
                var mi = updaterType.GetMethod("UpdateSource", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new[] { itemType }, null);
                if (isEnumerable)
                {
                    foreach (var item in (IEnumerable)waitUpdateItem)
                    {
                        mi.Invoke(updater, new object[] { item });
                    }
                }
                else
                {
                    mi.Invoke(updater, new object[] { waitUpdateItem });
                }
                iocResolver.Release(updater);
            }
            invocation.ReturnValue = cachedItem;
        }
    }
}
