using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCore;
using NLog;
using RedisMapper;
using ServiceStack.Redis;

namespace FrameMobile.Cache
{
    public sealed class RedisCacheHelper : ICacheManagerHelper
    {
        const string COUNT = "SYSTEM:KEYCOUNT";
        internal const string NULL_DATA = "$$NULL$$";
        internal const string NULL_DATA_GET = "\"$$NULL$$\"";

        #region Redis Client instance
        internal static IRedisClientsManager _redisClientManager;
        public static IRedisClientsManager RedisClientManager
        {
            get
            {
                if (_redisClientManager == null)
                {
                    var redisConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = AppConfigKeys.CACHE_MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = AppConfigKeys.CACHE_MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };


                    string[] readWriteHosts = AppConfigKeys.CACHE_REDIS_READ_WRITE_SERVERS.ConfigValue().Split(';');
                    string[] readOnlyHosts = AppConfigKeys.CACHE_REDIS_READONLY_SERVERS.ConfigValue().Split(';');

                    _redisClientManager = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, redisConfig);
                }

                return _redisClientManager;
            }
        }
        #endregion

        #region Add Data

        public void Add(string key, object value)
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                try
                {
                    Redis.Add(key, value);
                    Redis.IncrementValue(COUNT);
                }
                catch (RedisException ex)
                {
                    LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                }
            }
        }

        public void Add(string key, object value, DateTime? absoluteExpiration)
        {
            Add(key, value, absoluteExpiration - DateTime.Now);
        }

        public void Add(string key, object value, int timeoutSecs)
        {
            Add(key, value, new TimeSpan(0, 0, timeoutSecs));
        }

        public void Add(string key, object value, TimeSpan? slidingExpiration)
        {
            try
            {
                using (var Redis = RedisClientManager.GetClient())
                {
                    if (slidingExpiration != null)
                    {
                        Redis.Add(key, value, slidingExpiration.GetValueOrDefault());
                    }
                    else
                    {
                        Redis.Add(key, value);
                    }
                    Redis.IncrementValue(COUNT);
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        public void AddNullableData(string key, object value, int timeoutSeconds = 0)
        {
            try
            {
                using (var redis = RedisClientManager.GetClient())
                {
                    // check value
                    var span = new TimeSpan(0, 0, timeoutSeconds);

                    if (value == null) value = NULL_DATA;

                    if (timeoutSeconds <= 0) redis.Add(key, value);
                    else redis.Add(key, value, span);

                    redis.IncrementValue(COUNT);
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region GetData
        public T GetData<T>(string key)
        {
            try
            {
                using (var Redis = RedisClientManager.GetReadOnlyClient())
                {
                    return Redis.Get<T>(key);
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                return default(T);
            }
        }

        public object GetData(string key, Type dataType)
        {
            try
            {
                using (var Redis = RedisClientManager.GetReadOnlyClient())
                {
                    return ServiceStack.Text.JsonSerializer.DeserializeFromString(Redis.GetValue(key), dataType);
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }

        public object GetNullableData(string key, Type dataType)
        {
            try
            {
                var ret = default(object);
                using (var Redis = RedisClientManager.GetReadOnlyClient())
                {
                    var rawValue = Redis.GetValue(key);
                    if (!rawValue.EqualsOrdinalIgnoreCase(NULL_DATA_GET))
                    {
                        ret = ServiceStack.Text.JsonSerializer.DeserializeFromString(rawValue, dataType);
                    }

                    return ret;
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                return null;
            }
        }
        #endregion

        public void Remove(string key)
        {
            try
            {
                using (var Redis = RedisClientManager.GetClient())
                {
                    Redis.Remove(key);
                    Redis.DecrementValue(COUNT);
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        public bool Contains(string key)
        {
            try
            {
                using (var Redis = RedisClientManager.GetReadOnlyClient())
                {
                    return Redis.ContainsKey(key);
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    using (var Redis = RedisClientManager.GetReadOnlyClient())
                    {
                        return Redis.Get<int>(COUNT);
                    }
                }
                catch (RedisException ex)
                {
                    LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                    return 0;
                }
            }
        }

        public long Increment(string countID)
        {
            long Count = 0;
            using (var Redis = RedisClientManager.GetClient())
            {
                try
                {
                    Count = Redis.IncrementValue(countID);
                }
                catch (RedisException ex)
                {
                    LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
                }
            }
            return Count;
        }

        public void ClearServiceCache()
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                Redis.RemoveAll(Redis.SearchKeys("SVC:*"));
            }
        }

        public void Flush()
        {
            try
            {
                using (var Redis = RedisClientManager.GetClient())
                {
                    Redis.FlushDb();
                }
            }
            catch (RedisException ex)
            {
                LogManager.GetLogger("ErrorLogger").Error(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

    }
}
