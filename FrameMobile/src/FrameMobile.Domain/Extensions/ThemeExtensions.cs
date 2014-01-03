using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.Theme;
using StructureMap;

namespace FrameMobile.Domain
{
    public static class ThemeExtensions
    {
        public static int GetThemeConfsver<T>(this T source) where T : class, IMySQLModel, new()
        {
            if (source == null) return 0;

            var dbContextService = ObjectFactory.GetInstance<IThemeDbContextService>();
            var result = dbContextService.Single<ThemeConfig>(x => x.NameLowCase == source.GetType().Name.ToLower() && x.Status == 1);
            return result != null ? result.Version : 0;
        }

        public static IList<T> ReturnThemeInstance<T>(this T source, int cver, out int sver) where T : class, IMySQLModel, new()
        {
            var result = new List<T>();
            sver = 0;
            var version = GetThemeConfsver(source);
            if (version != cver)
            {
                var dbContextService = ObjectFactory.GetInstance<IThemeDbContextService>();
                sver = version;
                var type = typeof(T);
                var flag = (type == typeof(WallPaperCategory) || type == typeof(WallPaperSubCategory));
                result = flag ?
                    dbContextService.All<T>().ToList() : dbContextService.Find<T>(x => x.Status == 1).ToList();
                return result;
            }
            return result;
        }
    }
}
