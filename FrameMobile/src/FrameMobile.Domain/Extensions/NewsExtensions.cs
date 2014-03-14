using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Common;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.News;
using FrameMobile.Model.Radar;
using StructureMap;

namespace FrameMobile.Domain
{
    public static class NewsExtensions
    {
        public static int GetNewsConfsver<T>(this T source) where T : class, IMySQLModelBase, new()
        {
            if (source == null) return 0;

            var dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
            var type = typeof(T);
            var flag = (type == typeof(RadarCategory) || type == typeof(RadarElement));
            var configTableName = flag ? Const.NEWS_RADAR_CONFIG_TABLE_NAME : source.GetType().Name.ToLower();

            var result = dbContextService.Single<NewsConfig>(x => x.NameLowCase == configTableName && x.Status == 1);
            return result != null ? result.Version : 0;
        }

        public static IList<T> ReturnNewsInstance<T>(this T source, int cver, out int sver) where T : class, IMySQLModelBase, new()
        {
            var result = new List<T>();
            sver = cver;
            var version = GetNewsConfsver(source);
            if (version != cver)
            {
                var dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                sver = version;
                var type = typeof(T);
                var flag = (type == typeof(NewsCategory) || type == typeof(NewsSource) || type == typeof(NewsInfAddress));
                result = flag ?
                    dbContextService.All<T>().ToList() : dbContextService.Find<T>(x => x.Status == 1).ToList();
                return result;
            }
            return result;
        }

        public static DateTime NewsStartDefaultTime(this DateTime time)
        {
            var idealTime = DateTime.Now.AddDays(-3);
            if (time < idealTime)
            {
                return idealTime;
            }
            return time;
        }
    }
}
