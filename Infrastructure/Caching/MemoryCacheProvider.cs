using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private Dictionary<string/*key*/, Dictionary<string/*valkey*/, CacheItem>> cacheItems
            = new Dictionary<string, Dictionary<string, CacheItem>>();
        public bool Exists(string key)
        {
            return cacheItems.ContainsKey(key);
        }

        public bool Exists(string key, string valKey)
        {
            return (cacheItems.ContainsKey(key) && cacheItems[key].ContainsKey(valKey));
        }

        public object Get(string key, string valKey)
        {
            if(Exists(key,valKey))
            {
                CacheItem item = cacheItems[key][valKey];
                return item.Item;
            }
            return null;
        }

        public void Put(string key, string valKey, object value, CacheAttribute cacheAttribute)
        {
            Dictionary<string, CacheItem> items;
            if(cacheItems.ContainsKey(key))
            {
                items = cacheItems[key];
            }
            else
            {
                lock(cacheItems)
                {
                    if (cacheItems.ContainsKey(key))
                    {
                        items = cacheItems[key];
                    }
                    else
                    {
                        items = new Dictionary<string, CacheItem>();
                        cacheItems.Add(key, items);
                    }
                }
            }

            CacheItem item = new CacheItem();
            item.Item = value;
            if(cacheAttribute.Strategy==CacheStrategy.TimeOut)
            {
                item.TimeOutCallback = new System.Threading.Timer(
                    delegate(object state)
                    {
                        CacheItemIdentity identity = (CacheItemIdentity)state;
                        lock(this)
                        {
                            if(cacheItems.ContainsKey(identity.Key))
                            {
                                if(cacheItems[identity.Key].ContainsKey(identity.ValueKey))
                                {
                                    cacheItems[identity.Key][valKey].Dispose();
                                    cacheItems[identity.Key].Remove(identity.ValueKey);
                                }
                                
                                
                            }
                        }
                    },
                    new CacheItemIdentity { Key=key,ValueKey=valKey},
                    (int)cacheAttribute.Timeout.TotalMilliseconds,
                    0
                    );
            }
            if(items.ContainsKey(valKey))
            {
                lock (items)
                {
                    var oldItem = items[valKey];
                    if (oldItem.TimeOutCallback != null)
                    {
                        oldItem.Dispose();
                    }
                    items.Remove(valKey);
                }
            }
            items.Add(valKey, item);
        }

        public void Remove(string key)
        {
            Dictionary<string, CacheItem> items = null;
            if(cacheItems.TryGetValue(key,out items))
            {
                foreach(CacheItem ci in items.Values)
                {
                    if(ci.TimeOutCallback!=null)
                    {
                        ci.TimeOutCallback.Dispose();
                    }
                }
            }
            lock(this)
            {
                cacheItems.Remove(key);
            }
            
        }

        public void Remove(string key, string valueKey)
        {
            if(cacheItems.ContainsKey(key))
            {
                var cacheItem = cacheItems[key];
                lock(cacheItem)
                {
                    var item = cacheItem[valueKey];
                    if (item.TimeOutCallback != null)
                    {
                        item.TimeOutCallback.Dispose();
                    }
                    cacheItem.Remove(valueKey);
                }
            }
        }

        public void RemoveWithPrefix(string prefix)
        {
            var keys = cacheItems.Keys;
            List<string> removeKeys = new List<string>();
            foreach (var key in keys)
            {
                if (key.StartsWith(prefix))
                {
                    removeKeys.Add(key);
                    
                }
            }
            foreach(var key in removeKeys)
            {
                cacheItems.Remove(key);
            }
        }

        class CacheItem : IDisposable
        {
            public object Item { get; set; }
            public System.Threading.Timer TimeOutCallback { get; set; }

            private bool disposed = false;

            public void Dispose()
            {
                if(!disposed)
                {
                    TimeOutCallback.Dispose();
                    disposed = true;
                }
                
            }
        }
        class CacheItemIdentity
        {
            public string Key { get; set; }
            public string ValueKey { get; set; }
        }
    }
}
