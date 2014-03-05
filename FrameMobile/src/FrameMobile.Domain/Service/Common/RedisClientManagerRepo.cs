using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using NCore;
using RedisMapper;
using ServiceStack.Redis;

namespace FrameMobile.Domain.Service
{
    public class RedisClientManagerRepo : SingletonBase<RedisClientManagerRepo>
    {
        #region Client Manager

        public IRedisClientsManager NewsRedisClientManager
        {
            get
            {
                if (_newsRedisClientManager == null)
                {
                    _newsRedisClientManager = GetInstance(RedisClientManagerType.News);
                }

                return _newsRedisClientManager;
            }
        } IRedisClientsManager _newsRedisClientManager;

        public IRedisClientsManager ThemeRedisClientManager
        {
            get
            {
                if (_themeRedisClientManager == null)
                {
                    _themeRedisClientManager = GetInstance(RedisClientManagerType.Theme);
                }

                return _themeRedisClientManager;
            }
        } IRedisClientsManager _themeRedisClientManager;

        public IRedisClientsManager RedisClientManager
        {
            get
            {
                if (_redisClientManager == null)
                {
                    _redisClientManager = GetInstance(RedisClientManagerType.Mixed);
                }

                return _redisClientManager;
            }
        } IRedisClientsManager _redisClientManager;

        #endregion

        #region Method

        private IRedisClientsManager GetInstance(RedisClientManagerType type)
        {
            var manager = default(IRedisClientsManager);

            switch (type)
            {
                case RedisClientManagerType.News:
                    var newsConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = ConfigKeys.NEWS_MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = ConfigKeys.NEWS_MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };

                    string[] newsReadWriteHosts = ConfigKeys.NEWS_REDIS_READ_WRITE_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);
                    string[] newsReadOnlyHosts = ConfigKeys.NEWS_REDIS_READONLY_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);

                    manager = new PooledRedisClientManager(newsReadWriteHosts, newsReadWriteHosts, newsConfig);

                    break;

                case RedisClientManagerType.Theme:
                    var themeConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = ConfigKeys.THEME_MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = ConfigKeys.THEME_MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };

                    string[] themeReadWriteHosts = ConfigKeys.THEME_REDIS_READ_WRITE_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);
                    string[] themeRadOnlyHosts = ConfigKeys.THEME_REDIS_READONLY_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);

                    manager = new PooledRedisClientManager(themeReadWriteHosts, themeReadWriteHosts, themeConfig);

                    break;

                case RedisClientManagerType.Mixed:
                    var mixedConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = AppConfigKeys.MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = AppConfigKeys.MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };

                    string[] readWriteHostsMixed = AppConfigKeys.REDIS_READ_WRITE_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);
                    string[] readOnlyHostsMixed = AppConfigKeys.REDIS_READONLY_SERVERS.ConfigValue().Split(ASCII.SEMICOLON_CHAR);

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
