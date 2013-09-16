using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;

namespace FrameMobile.Domain
{
    public static class NewsExtensions
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
            var random = new Random();
            return random.Next(0, source.Count);
        }

        public static int CheckVersion<T>(this T source) where T : class, IMySQLModel, new()
        {
            if (source == null) return 0;

            var dbcontext = ObjectFactory.GetInstance<IDbContext>();
            var result = dbcontext.Single<NewsConfig>(x => x.Name == source.GetType().Name && x.Status == 1);
            return result != null ? result.Version : 0;
        }

        public static IList<T> GetResultByVer<T>(this T source, int cver, out int sver) where T : class, IMySQLModel, new()
        {
            var result = new List<T>();
            sver = 0;
            var version = CheckVersion(source);
            if (version != cver)
            {
                var dbContextService = ObjectFactory.GetInstance<IDbContext>();
                sver = version;
                result = dbContextService.Find<T>(x => x.Status == 1).ToList();
                return result;
            }
            return result;
        }
    }
}
