using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FrameMobile.Cache;

namespace FrameMobile.Domain.Service
{
    public class CacheService : ICacheService
    {
        public void Clear(IRedisCacheService cacheService)
        {
            cacheService.ClearServiceCache();
            List<string> toRemove = new List<string>();
            foreach (DictionaryEntry cacheItem in HttpRuntime.Cache)
            {
                toRemove.Add(cacheItem.Key.ToString());
            }
            foreach (string key in toRemove)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        public void ClearAll(IRedisCacheService cacheService)
        {
            cacheService.Flush();
        }
    }
}
