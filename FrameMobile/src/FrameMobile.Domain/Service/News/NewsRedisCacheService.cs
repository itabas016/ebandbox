using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Cache;
using RedisMapper;
using ServiceStack.Redis;

namespace FrameMobile.Domain.Service
{
    public class NewsRedisCacheService : RedisCacheService, INewsRedisCacheService
    {
        public override IRedisClientsManager RedisClientManager
        {
            get
            {
                return RedisClientCacheManagerRepo.Instance.NewsRedisClientCacheManager;
            }
        }
    }
}
