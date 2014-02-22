using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.Theme;
using StructureMap;
using FrameMobile.Common;
using NCore;

namespace FrameMobile.Domain
{
    public static class ThemeExtensions
    {
        public static int GetThemeConfsver<T>(this T source) where T : class, IMySQLModelBase, new()
        {
            if (source == null) return 0;

            var dbContextService = ObjectFactory.GetInstance<IThemeDbContextService>();
            var result = dbContextService.Single<ThemeConfig>(x => x.NameLowCase == source.GetType().Name.ToLower() && x.Status == 1);
            return result != null ? result.Version : 0;
        }

        public static IList<T> ReturnThemeInstance<T>(this T source, int cver, out int sver) where T : class, IMySQLModelBase, new()
        {
            var result = new List<T>();
            sver = cver;
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

        public static string GetCompletePath(this string fileName, string fileType, string resolution)
        {
            var fileNamePrefix = string.Empty;
            var thumbnailPrefix = ConfigKeys.TYD_WALLPAPER_THUMBNAIL_IMAGE_PREFIX.ConfigValue();
            var originalPrefix = ConfigKeys.TYD_WALLPAPER_ORIGINAL_IMAGE_PREFIX.ConfigValue();

            switch (fileType)
            {
                case Const.WALLPAPER_THUMBNAIL:
                    fileNamePrefix = thumbnailPrefix;
                    break;
                case Const.WALLPAPER_ORIGINAL:
                    fileNamePrefix = originalPrefix;
                    break;
                default:
                    break;
            }

            var result = string.Format("{0}_{1}_{2}", fileNamePrefix, resolution, fileName);
            return result;
        }

        public static string GetCompleteThumbnailUrl(this string thumbnailName, MobileParam mobileParams)
        {
            var resolution = mobileParams.Resolution.ToLower();
            if (string.IsNullOrEmpty(resolution))
            {
                return string.Format("{0}{1}", ConfigKeys.TYD_WALLPAPER_THUMBNAIL_IMAGE_PREFIX.ConfigValue(), thumbnailName);
            }
            return string.Format("{0}{1}_{2}", ConfigKeys.TYD_WALLPAPER_THUMBNAIL_IMAGE_PREFIX.ConfigValue(), resolution, thumbnailName);
        }

        public static string GetCompleteOriginalUrl(this string originalName, MobileParam mobileParams)
        {
            var resolution = mobileParams.Resolution.ToLower();
            if (string.IsNullOrEmpty(resolution))
            {
                return string.Format("{0}{1}", ConfigKeys.TYD_WALLPAPER_ORIGINAL_IMAGE_PREFIX.ConfigValue(), originalName);
            }
            return string.Format("{0}{1}_{2}", ConfigKeys.TYD_WALLPAPER_ORIGINAL_IMAGE_PREFIX.ConfigValue(), resolution, originalName);
        }
    }
}
