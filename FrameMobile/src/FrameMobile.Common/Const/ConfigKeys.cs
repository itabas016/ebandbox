using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Common
{
    public class ConfigKeys
    {
        #region System

        public const string ENABLE_SNAP = "EnableSNAP";

        public const string SERVICE_CACHE_TIMEOUT_SECONDS = "ServiceCacheTimeOutSeconds";

        public const string USING_SHARED_RESOURCE_FOLDER = "UsingSharedResourceFolder";

        #region Redis

        public const string NEWS_REDIS_READ_WRITE_SERVERS = "news_redis_read_write_servers";

        public const string NEWS_REDIS_READONLY_SERVERS = "news_redis_readonly_servers";

        public const string CACHE_NEWS_REDIS_READ_WRITE_SERVERS = "cache_news_redis_read_write_servers";

        public const string CACHE_NEWS_REDIS_READONLY_SERVERS = "cache_news_redis_readonly_servers";

        public const string THEME_REDIS_READ_WRITE_SERVERS = "theme_redis_read_write_servers";

        public const string THEME_REDIS_READONLY_SERVERS = "theme_redis_readonly_servers";

        public const string CACHE_THEME_REDIS_READ_WRITE_SERVERS = "cache_theme_redis_read_write_servers";

        public const string CACHE_THEME_REDIS_READONLY_SERVERS = "cache_theme_redis_readonly_servers";

        public const string NEWS_MAX_READ_POOL_SIZE = "news_MaxReadPoolSize";

        public const string NEWS_MAX_WRITE_POOL_SIZE = "news_MaxWritePoolSize";

        public const string CACHE_NEWS_MAX_READ_POOL_SIZE = "cache_news_MaxReadPoolSize";

        public const string CACHE_NEWS_MAX_WRITE_POOL_SIZE = "cache_news_MaxWritePoolSize";

        public const string THEME_MAX_READ_POOL_SIZE = "theme_MaxReadPoolSize";

        public const string THEME_MAX_WRITE_POOL_SIZE = "theme_MaxWritePoolSize";

        public const string CACHE_THEME_MAX_READ_POOL_SIZE = "cache_theme_MaxReadPoolSize";

        public const string CACHE_THEME_MAX_WRITE_POOL_SIZE = "cache_theme_MaxWritePoolSize";

        #endregion

        #endregion

        #region TouTiao

        public const string TYD_NEWS_RESOURCES_DIR_ROOT = "TYD_News_Resources_Dir_Root";

        public const string TYD_NEWS_RESOURCES_DIR_ROOT_CLOSE = "TYD_News_Resources_Dir_Root_Close";

        public const string TYD_NEWS_IMAGE_FILE_URL = "TYD_News_Image_File_URL";

        public const string TYD_NEWS_TOUTIAO_PARTNER = "TYD_News_TouTiao_Partner";

        public const string TYD_NEWS_TOUTIAO_SECURE_KEY = "TYD_News_TouTiao_Secure_Key";

        public const string TYD_NEWS_TOUTIAO_REQUEST_URL = "TYD_News_TouTiao_Request_URL";

        public const string TYD_NEWS_TOUTIAO_REQUEST_COUNT = "TYD_News_TouTiao_Request_Count";

        public const string DEMO_TOUTIAO_FILE_PATH_ROOT = "Demo_TouTiao_File_Path_Root";

        public const string TYD_NEWS_APP_DOWNLOAD_PREFIX_URL = "TYD_News_App_Download_Prefix_URL";

        public const string TYD_NEWS_LOGO_IMAGE_PREFIX = "TYD_News_Logo_Image_Prefix";

        public const string TYD_AD_EXTRA_RATIO_DEFAULT_VALUE = "TYD_AD_Extra_Ratio_Default_Value";

        #endregion

        #region Theme

        public const string TYD_THEME_RESOURCES_DIR_ROOT = "TYD_Theme_Resources_Dir_Root";

        public const string TYD_WALLPAPER_LOGO_IMAGE_PREFIX = "TYD_WallPaper_Logo_Image_Prefix";

        public const string TYD_WALLPAPER_THUMBNAIL_IMAGE_PREFIX = "TYD_WallPaper_Thumbnail_Image_Prefix";

        public const string TYD_WALLPAPER_ORIGINAL_IMAGE_PREFIX = "TYD_WallPaper_Original_Image_Prefix";

        #endregion

        #region Tencent
        //Tencent

        public const string TYD_NEWS_TENCENT_REQUEST_URL = "TYD_News_Tencent_Request_URL";

        public const string TYD_NEWS_TENCENT_APP_KEY = "TYD_News_Tencent_App_Key";

        //当前只为10
        public const string TYD_NEWS_TENCENT_REQUEST_COUNT = "TYD_News_Tencent_Request_Count";

        #endregion

        #region MainTain

        //Clean

        public const string CLEANUP_NEWS_CONTENT_DAYS_AGO_VALUE = "Cleanup_News_Content_Days_Ago_Value";

        //Update

        public const string UPDATE_NEWS_SAME_PUBLISH_TIME_LIMIT = "Update_News_Same_PublishTime_Limit";

        public const string UPDATE_NEWS_START_PUBLISH_TIME_HOUR = "Update_News_Start_PublishTime_Hour";

        #endregion
    }
}
