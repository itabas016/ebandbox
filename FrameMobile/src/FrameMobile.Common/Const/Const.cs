using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Common
{
    public class Const
    {
        #region TouTiao

        public const string NEWS_SOURCES_NAME_TouTiao = "今日头条";

        public const string NEWS_SOURCES_NAME_LOW_CASE_TouTiao = "toutiao";

        public const string NEWS_SOURCES_PKG_NAME_TouTiao = "com.ss.android.article.news";

        public const int NEWS_HD_RESOLUTION_WIDTH = 720;

        public const int NEWS_NORMAL_RESOLUTION_WIDTH = 480;

        public const string NEWS_RESOURCES_FOLDER_NAME = "NewsResources";

        public const string NEWS_LOGOS_FOLDER_NAME = "Logos";

        #endregion

        #region Theme

        public const string WALLPAPER_THUMBNAIL = "thumbnail";

        public const string WALLPAPER_ORIGINAL = "original";

        public const string THEME_RESOURCES_FOLDER_NAME = "ThemeResources";

        public const string THEME_LOGOS_FOLDER_NAME = "Logos";

        public const string THEME_THUMBNAILS_FOLDER_NAME = "Thumbnails";

        public const string THEME_ORIGINALS_FOLDER_NAME = "Originals";

        #endregion

        #region Sort Field

        public const string SORT_PUBLISHTIME = "PublishTime";

        public const string SORT_DOWNLOADNUMBER = "DownloadNumber";

        #endregion

        #region System

        public const string FRAME_MODEL_MASTER = "FrameMobile.Model";

        public const string NEWS_METHOD_NAME_GETNEWSCONTENTVIEWLIST = "getnewscontentviewlist";

        public const string NEWS_METHOD_NAME_GETCONTENTVIEWLIST = "getcontentviewlist";

        public const string NEWS_METHOD_NAME_GETNEWSCOLLECTIONVIEW = "getnewscollectionview";

        public const string NEWS_METHOD_NAME_GETIMAGETYPEBYRESOLUTION = "getimageurltypebyresolution";

        public const string NEWS_METHOD_NAME_GETEXTRARATIOBYCHANNEL = "getextraratiobychannel";

        public const string NEWS_METHOD_NAME_GETEXTRAAPPVIEWLIST = "getextraappviewlist";

        public const string NEWS_METHOD_NAME_GETNEWSRADARVIEWLIST = "getnewsradarviewlist";

        public const string WALLPAPER_METHOD_NAME_GETMOBILEPROPERTY = "getmobileproperty";

        public const string WALLPAPER_METHOD_NAME_GETWALLPAPERVIEWLIST = "getwallpaperviewlist";

        public const string WALLPAPER_METHOD_NAME_GETWALLPAPERVIEWDETAIL = "getwallpaperviewdetail";

        #region Helper

        public const string COMMON_HELPER_METHOD_NAME_GETMOBILEPROPERTY = "getmobileproperty";

        public const string NEWS_HELPER_METHOD_GETOLDESTNEWSCONTENTVIEW = "getoldestnewscontentview";

        public const string NEWS_HELPER_METHOD_GETLATESTNEWSCONTENTVIEW = "getlatestnewscontentview";

        public const string NEWS_HELPER_METHOD_GETLOCALCONTENTVIEWLIST = "getlocalcontentviewlist";

        public const string WALLPAPER_HELPER_METHOD_NAME_GETLATESTWALLPAPERVIEWLIST = "getlatestwallpaperviewlist";

        public const string WALLPAPER_HELPER_METHOD_NAME_GETHOTTESTWALLPAPERVIEWLIST = "gethottestwallpaperviewlist";


        #endregion

        #endregion

        #region Account

        public const string SUPER_ADMIN_GROUPID = "1";
        
        #endregion
        
        #region Relation
        
        public const string NEWS_RADAR_CONFIG_TABLE_NAME = "newsradar";

        #endregion
    }
}
