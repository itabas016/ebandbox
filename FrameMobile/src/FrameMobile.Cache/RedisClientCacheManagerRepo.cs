using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using NCore;
using RedisMapper;
using ServiceStack.Redis;

namespace FrameMobile.Cache
{
    public class RedisClientCacheManagerRepo : SingletonBase<RedisClientCacheManagerRepo>
    {
        #region Client Manager

        public IRedisClientsManager NewsRedisClientCacheManager
        {
            get
            {
                if (_newsRedisClientCacheManager == null)
                {
                    _newsRedisClientCacheManager = GetInstance(RedisClientManagerType.NewsCache);
                }

                return _newsRedisClientCacheManager;
            }
        } IRedisClientsManager _newsRedisClientCacheManager;

        public IRedisClientsManager ThemeRedisClientCacheManager
        {
            get
            {
                if (_themeRedisClientCacheManager == null)
                {
                    _themeRedisClientCacheManager = GetInstance(RedisClientManagerType.ThemeCache);
                }

                return _themeRedisClientCacheManager;
            }
        } IRedisClientsManager _themeRedisClientCacheManager;

        public IRedisClientsManager RedisClientCacheManager
        {
            get
            {
                if (_redisClientCacheManager == null)
                {
                    _redisClientCacheManager = GetInstance(RedisClientManagerType.MixedCache);
                }

                return _redisClientCacheManager;
            }
        } IRedisClientsManager _redisClientCacheManager;

        #endregion

        #region Method

        private IRedisClientsManager GetInstance(RedisClientManagerType type)
        {
            var manager = default(IRedisClientsManager);

            switch (type)
            {
                case RedisClientManagerType.NewsCache:
                    var newsConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = ConfigKeys.CACHE_NEWS_MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = ConfigKeys.CACHE_NEWS_MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };

                    string[] newsReadWriteHosts = ConfigKeys.CACHE_NEWS_REDIS_READ_WRITE_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);
                    string[] newsReadOnlyHosts = ConfigKeys.CACHE_NEWS_REDIS_READONLY_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);

                    manager = new PooledRedisClientManager(newsReadWriteHosts, newsReadWriteHosts, newsConfig);

                    break;

                case RedisClientManagerType.ThemeCache:
                    var themeConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = ConfigKeys.CACHE_THEME_MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = ConfigKeys.CACHE_THEME_MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };

                    string[] themeReadWriteHosts = ConfigKeys.CACHE_THEME_REDIS_READ_WRITE_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);
                    string[] themeRadOnlyHosts = ConfigKeys.CACHE_THEME_REDIS_READONLY_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);

                    manager = new PooledRedisClientManager(themeReadWriteHosts, themeReadWriteHosts, themeConfig);

                    break;

                case RedisClientManagerType.MixedCache:
                    var mixedConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = AppConfigKeys.CACHE_MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = AppConfigKeys.CACHE_MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };

                    string[] readWriteHostsMixed = AppConfigKeys.CACHE_REDIS_READ_WRITE_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);
                    string[] readOnlyHostsMixed = AppConfigKeys.CACHE_REDIS_READONLY_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);

                    manager = new PooledRedisClientManager(readWriteHostsMixed, readOnlyHostsMixed, mixedConfig);

                    break;

                default:
                    break;
            }

            return manager;
        }

        #endregion
    }
}
