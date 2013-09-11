using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Core
{
    public sealed class CacheHelper
    {
        private static ICacheManager cacheManager = CacheFactory.GetCacheManager();

        public static ICacheManager CacheManagerInstance
        {
            get
            {
                return cacheManager;
            }
        }

        public static void Add(string key, object value)
        {
            CacheManagerInstance.Add(key, value);
        }

        public static void Add(string key, object value, CachePriority priority, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
        {
            CacheItemPriority p = (CacheItemPriority)Enum.Parse(typeof(CacheItemPriority), priority.ToString());

            if (absoluteExpiration != null & slidingExpiration != null)
            {
                CacheManagerInstance.Add(key, value, p, null, new AbsoluteTime(absoluteExpiration.Value), new SlidingTime(slidingExpiration.Value));
            }
            else if (absoluteExpiration != null)
            {
                CacheManagerInstance.Add(key, value, p, null, new AbsoluteTime(absoluteExpiration.Value));
            }
            else if (slidingExpiration != null)
            {
                CacheManagerInstance.Add(key, value, p, null, new SlidingTime(slidingExpiration.Value));
            }
            else
            {
                CacheManagerInstance.Add(key, value, p, null);
            }
        }

        public static void Remove(string key)
        {
            CacheManagerInstance.Remove(key);
        }

        public static bool Contains(string key)
        {
            return CacheManagerInstance.Contains(key);
        }

        public static int Count
        {
            get
            {
                return CacheManagerInstance.Count;
            }
        }

        public static void Flush()
        {
            CacheManagerInstance.Flush();
        }

        public static T GetData<T>(string key)
        {
            return (T)CacheManagerInstance.GetData(key);
        }

        public static object GetData(string key)
        {
            return CacheManagerInstance.GetData(key);
        }
    }

    public enum CachePriority
    {
        None = 0,
        Low = 1,
        Normal = 2,
        High = 3,
        NotRemovable = 4
    }
}
