using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Cache;
using FrameMobile.Common;
using FrameMobile.Domain.Service;
using StructureMap;

namespace FrameMobile.Domain
{
    public static class ServiceExtensions
    {
        public static TOutput To<TOutput>(this object source)
        {
            if (source == null) return default(TOutput);

            return (TOutput)Mapper.Map(source, source.GetType(), typeof(TOutput));
        }

        public static IList<TOutput> To<TOutput>(this IList<object> source)
        {
            if (source == null) return default(IList<TOutput>);

            return (IList<TOutput>)Mapper.Map(source, source.GetType(), typeof(TOutput));
        }

        public static int RandomInt<T>(this IList<T> source)
        {
            if (source == null) return default(int);
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(1, source.Count + 1);
        }

        public static IRedisCacheService RedisCacheServiceFactory(this RedisClientManagerType clientType)
        {
            var redisCacheService = default(IRedisCacheService);
            switch (clientType)
            {
                case RedisClientManagerType.NewsCache:
                    redisCacheService = ObjectFactory.GetInstance<INewsRedisCacheService>();
                    break;
                case RedisClientManagerType.ThemeCache:
                    redisCacheService = ObjectFactory.GetInstance<IThemeRedisCacheService>();
                    break;
                case RedisClientManagerType.MixedCache:
                    redisCacheService = ObjectFactory.GetInstance<IRedisCacheService>();
                    break;
                default:
                    redisCacheService = ObjectFactory.GetInstance<IRedisCacheService>();
                    break;
            }
            return redisCacheService;
        }
    }
}
