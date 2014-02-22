using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.Radar;
using StructureMap;

namespace FrameMobile.Domain
{
    public static class RadarExtensions
    {
        public static IList<T> ReturnRadarInstance<T>(this T source, int cver, out int sver) where T : class, IMySQLModelBase, new()
        {
            var result = new List<T>();
            sver = cver;
            var version = source.GetNewsConfsver();
            if (version != cver)
            {
                var dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                sver = version;
                var type = typeof(T);
                var flag = (type == typeof(RadarCategory) || type == typeof(RadarElement));
                result = flag ?
                    dbContextService.All<T>().ToList() : dbContextService.Find<T>(x => x.Status == 1).ToList();
                return result;
            }
            return result;
        }
    }
}
