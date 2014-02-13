using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FrameMobile.Common;

namespace FrameMobile.Domain
{
    public class ResourcesFilePathHelper
    {
        public static string NewsResourcesBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Const.NEWS_RESOURCES_FOLDER_NAME);
        public static string ThemeResourcesBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Const.THEME_RESOURCES_FOLDER_NAME);

        public static string NewsRadarLogoPath = Path.Combine(NewsResourcesBasePath, Const.NEWS_RADAR_LOGOS_FOLDER_NAME);
        public static string ThemeLogoPath = Path.Combine(ThemeResourcesBasePath, Const.THEME_LOGOS_FOLDER_NAME);
        public static string ThemeThumbnailPath = Path.Combine(ThemeResourcesBasePath, Const.THEME_THUMBNAILS_FOLDER_NAME);
        public static string ThemeOriginalPath = Path.Combine(ThemeResourcesBasePath, Const.THEME_ORIGINALS_FOLDER_NAME);
    }
}
