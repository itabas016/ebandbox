using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Cache;
using RedisMapper;
using NCore;
using FrameMobile.Core;

namespace FrameMobile.Domain.Interceptor
{
    public abstract class CacheDecoratorBase
    {
        internal abstract string CachePrefix { get; }

        private ICacheManagerHelper RedisCacheManagerHelper;
        public CacheDecoratorBase(ICacheManagerHelper cacheManagerHelper)
        {
            RedisCacheManagerHelper = cacheManagerHelper;
        }

        protected T GetData<T>(string key, Func<T> getData, int expiredMinutes = 10)
        {
            T ret = default(T);
            if (string.IsNullOrWhiteSpace(AppConfigKeys.CACHE_REDIS_READ_WRITE_SERVERS.ConfigValue()))
            {
                ret = CacheHelper.GetData<T>(key);
                if (ret == null) ret = getData();
                if (ret != null) AddToCache(ret, key, expiredMinutes);
            }
            else
            {
                var contains = RedisCacheManagerHelper.Contains(key);
                if (contains)
                {
                    var cachedValue = RedisCacheManagerHelper.GetNullableData(key, typeof(T));
                    if (cachedValue != null)
                    {
                        ret = (T)cachedValue;
                    }
                }
                else
                {
                    ret = getData();
                    AddToCache(ret, key, expiredMinutes);
                }
            }

            return ret;
        }

        protected virtual void RemoveCache(string key)
        {
            if (string.IsNullOrWhiteSpace(AppConfigKeys.CACHE_REDIS_READ_WRITE_SERVERS.ConfigValue()))
            {
                CacheHelper.Remove(key);
            }
            else
            {
                RedisCacheManagerHelper.Remove(key);
            }
        }

        protected void AddToCache(object value, string key, int expiredMinutes)
        {
            if (string.IsNullOrWhiteSpace(AppConfigKeys.CACHE_REDIS_READ_WRITE_SERVERS.ConfigValue()))
            {
                CacheHelper.Add(key, value, CachePriority.Normal, DateTime.Now.AddMinutes(expiredMinutes), null);
            }
            else
            {
                RedisCacheManagerHelper.AddNullableData(key, value, expiredMinutes * 60);
            }
        }
    }
}
