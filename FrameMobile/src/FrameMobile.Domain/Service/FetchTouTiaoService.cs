using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model.ThirdPart;
using NCore;
using Newtonsoft.Json;
using FrameMobile.Model.News;
using System.IO;
using StructureMap;
using System.Web;

namespace FrameMobile.Domain.Service
{
    public class FetchTouTiaoService
    {
        #region Prop

        private IDbContextService _dbContextService;
        public IDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        public static string NEWS_RESOURCES_DIR_ROOT = ConfigKeys.TYD_NEWS_RESOURCES_DIR_ROOT.ConfigValue();

        public int NEWS_CATEGORY_REQUEST_COUNT = ConfigKeys.TYD_NEWS_TOUTIAO_REQUEST_COUNT.ConfigValue().ToInt32();

        public string NEWS_IMAGE_DIR_BASE = string.Format("{0}\\Original\\", NEWS_RESOURCES_DIR_ROOT);

        public string NEWS_DEST_HD_IMAGE_DIR_BASE = string.Format("{0}\\Images\\720\\", NEWS_RESOURCES_DIR_ROOT);

        public string NEWS_DEST_NORMAL_IMAGE_DIR_BASE = string.Format("{0}\\Images\\480\\", NEWS_RESOURCES_DIR_ROOT);

        public string NEWS_IMAGE_FILE_URL = ConfigKeys.TYD_NEWS_IMAGE_FILE_URL.ConfigValue();

        public const string NEWS_SOURCES_NAME = "今日头条";

        public const string NEWS_SOURCES_NAME_LOW_CASE = "toutiao";

        public const string NEWS_SOURCES_PKG_NAME = "com.ss.android.article.news";

        #endregion

        #region Ctor
        public FetchTouTiaoService(IDbContextService dbContextService)
        {
            this.dbContextService = dbContextService;
        }
        #endregion

        #region Method

        public TouTiaoParameter GenerateParam()
        {
            var param = new TouTiaoParameter();
            return param;
        }

        public string Request(string category, long cursor = 0, int count = 50)
        {
            var param = GenerateParam();
            var request_url = ConfigKeys.TYD_NEWS_TOUTIAO_REQUEST_URL.ConfigValue();
            count = NEWS_CATEGORY_REQUEST_COUNT;
            param.Category = category;

            var query_url = string.Format("nonce={0}&category={1}&timestamp={2}&signature={3}&partner={4}&cursor={5}&count={6}",
            param.Nonce, param.Category, param.Timestamp, param.Signature, param.Partner, cursor, count);

            var response = HttpHelper.HttpGet(request_url, query_url);
            return response;
        }

        public TouTiaoResult DeserializeTouTiao(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }
            var instance = JsonConvert.DeserializeObject<TouTiaoResult>(response);
            return instance;
        }

        public List<TouTiaoContent> Anlynaze(TouTiaoResult toutiaoResult, out long cursor)
        {
            cursor = 0;
            if (toutiaoResult == null)
            {
                return null;
            }
            if (toutiaoResult.ret == 0 && toutiaoResult.DataByCursor != null)
            {
                cursor = toutiaoResult.DataByCursor.Cursor;
                var result = toutiaoResult.DataByCursor.ContentList;
                return result;
            }
            return null;
        }

        public long GetCurrentCursor(string category)
        {
            //var cursor = GetCategoryCursor(category);
            long cursor = 0;
            var response = Request(category);
            var instance = DeserializeTouTiao(response);
            var contentList = Anlynaze(instance, out cursor);

            SaveContentList(category, contentList);
            return cursor;
        }

        public void SaveContentList(string category, List<TouTiaoContent> contentList)
        {
            if (contentList != null && contentList.Count > 0)
            {
                var no_repeat = 0;
                foreach (var item_content in contentList)
                {
                    var touTiaoModel = item_content.To<NewsContentRefactor>();
                    touTiaoModel.CategoryId = GetCategoryId(category);
                    touTiaoModel.SubCategoryId = GetSubCategoryId(category);
                    var exist = dbContextService.Exists<NewsContentRefactor>(x => x.NewsId == item_content.NewsId);
                    if (!exist)
                    {
                        no_repeat++;
                        touTiaoModel = ImageSave(item_content, touTiaoModel);
                        dbContextService.Add<NewsContentRefactor>(touTiaoModel);
                    }
                }
                NLogHelper.WriteInfo(string.Format("{0} content count is {1}. Not repeat content count is {2} ", category, contentList.Count, no_repeat));
            }
        }

        public void SingleCapture(string categoryName)
        {
            var cursor = GetCurrentCursor(categoryName);
            UpdateCategoryCursor(categoryName, cursor);
            NLogHelper.WriteInfo(string.Format("{0} cursor is {1}", categoryName, cursor));
        }

        public void Capture()
        {
            try
            {
                var categoryList = Enum.GetNames(typeof(TouTiaoCategory));
                foreach (var item_category in categoryList)
                {
                    SingleCapture(item_category);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.WriteError(ex.Message);
                NLogHelper.WriteError(ex.StackTrace);
            }
        }

        #region Helper

        public void UpdateCategoryCursor(string categoryName, long cursor)
        {
            var subcategory = dbContextService.Single<NewsSubCategory>(x => x.Name == categoryName);
            if (subcategory != null && cursor != subcategory.Cursor)
            {
                subcategory.Cursor = cursor;
                dbContextService.Update<NewsSubCategory>(subcategory);
            }
        }

        public long GetCategoryCursor(string categoryName)
        {
            var subcategory = dbContextService.Single<NewsSubCategory>(x => x.Name == categoryName);

            if (subcategory == null)
            {
                var subCategoryId = GetSubCategoryId(categoryName, true);
                return 0;
            }
            return subcategory.Cursor;
        }

        public int GetCategoryId(string categoryName)
        {
            var subCategory = dbContextService.Single<NewsSubCategory>(x => x.Name == categoryName);
            if (subCategory == null)
            {
                return GetCategoryId(categoryName, true);
            }
            return subCategory.CategoryId;
        }

        public int GetSubCategoryId(string categoryName)
        {
            var subCategory = dbContextService.Single<NewsSubCategory>(x => x.Name == categoryName);
            if (subCategory == null)
            {
                return GetSubCategoryId(categoryName, true);
            }
            return subCategory.Id;
        }

        public int GetCategoryId(string categoryName, bool emptyObj)
        {
            if (emptyObj)
            {
                var newsSubCategory = new NewsSubCategory();

                newsSubCategory.SourceId = GetSourceId();

                newsSubCategory = MatchCategory(newsSubCategory, categoryName);

                var subCategoryId = dbContextService.Add<NewsSubCategory>(newsSubCategory);

                return newsSubCategory.CategoryId;
            }
            return 0;
        }

        public int GetSubCategoryId(string categoryName, bool emptyObj)
        {
            if (emptyObj)
            {
                var newsSubCategory = new NewsSubCategory();

                newsSubCategory.SourceId = GetSourceId();

                newsSubCategory = MatchCategory(newsSubCategory, categoryName);

                var subCategoryId = dbContextService.Add<NewsSubCategory>(newsSubCategory);

                return (int)subCategoryId;
            }
            return 0;
        }

        public int GetSourceId()
        {
            var source = dbContextService.Single<NewsSource>(x => x.NameLowCase == NEWS_SOURCES_NAME_LOW_CASE);
            if (source == null)
            {
                var sourceId = dbContextService.Add<NewsSource>(new NewsSource
                {
                    Name = Const.NEWS_SOURCES_NAME_TouTiao,
                    NameLowCase = Const.NEWS_SOURCES_NAME_LOW_CASE_TouTiao,
                    PackageName = Const.NEWS_SOURCES_PKG_NAME_TouTiao
                });
                return (int)sourceId;
            }
            return source.Id;
        }

        public NewsSubCategory MatchCategory(NewsSubCategory newsSubCategory, string categoryName)
        {
            newsSubCategory.Name = categoryName;

            #region case
            switch (categoryName)
            {
                case "news_hot":
                    newsSubCategory.DisplayName = "热门";
                    newsSubCategory.CategoryId = 1;

                    break;
                case "news_finance":
                    newsSubCategory.DisplayName = "财经";
                    newsSubCategory.CategoryId = 5;
                    break;
                case "news_entertainment":
                    newsSubCategory.DisplayName = "娱乐";
                    newsSubCategory.CategoryId = 3;
                    break;
                case "news_tech":
                    newsSubCategory.DisplayName = "科技";
                    newsSubCategory.CategoryId = 2;
                    break;
                case "news_story":
                    newsSubCategory.DisplayName = "故事";
                    newsSubCategory.CategoryId = 6;
                    break;
                case "news_discovery":
                    newsSubCategory.DisplayName = "探索";
                    newsSubCategory.CategoryId = 6;
                    break;
                case "news_history":
                    newsSubCategory.DisplayName = "历史";
                    newsSubCategory.CategoryId = 6;
                    break;
                case "news_regimen":
                    newsSubCategory.DisplayName = "养生";
                    newsSubCategory.CategoryId = 6;
                    break;
                case "positive":
                    newsSubCategory.DisplayName = "正能量";
                    newsSubCategory.CategoryId = 6;
                    break;
                default:
                    newsSubCategory.CategoryId = 0;
                    break;
            }
            #endregion

            return newsSubCategory;
        }

        public string ImageListSave(TouTiaoContent content)
        {
            var imageList = content.ImageList;
            if (imageList != null && imageList.Count > 0)
            {
                //NLogHelper.WriteInfo(string.Format("images count is {0}", imageList.Count));
                var newsId = content.NewsId;

                var sb = new StringBuilder();
                //Save first image
                var imageId = SingleImageSave(imageList[0], newsId);
                //多个图片Id用;隔开

                sb.Append(imageId + ASCII.SEMICOLON);
                return sb.ToString().TrimEnd(ASCII.SEMICOLON_CHAR);
            }
            return string.Empty;
        }

        public int SingleImageSave(TouTiaoImageInfo imageInfo, long newsId)
        {
            var destImage = imageInfo.To<NewsImageInfo>();
            destImage.NewsId = newsId;
            if (imageInfo.UrlList != null && imageInfo.UrlList.Count > 0)
            {
                //download single one from any one url
                var single_img_url = imageInfo.UrlList[0];
                MakeSureDIRExist(NEWS_IMAGE_DIR_BASE);
                MakeSureDIRExist(NEWS_DEST_HD_IMAGE_DIR_BASE);
                MakeSureDIRExist(NEWS_DEST_NORMAL_IMAGE_DIR_BASE);
                var fileNamePath = HttpHelper.DownloadFile(single_img_url, Path.Combine(NEWS_IMAGE_DIR_BASE, GetFileNameFromURL(single_img_url)));

                var destFileNameHD = ImageHelper.ResizedHD(fileNamePath, NEWS_DEST_HD_IMAGE_DIR_BASE, newsId);
                var destFileNameNormal = ImageHelper.ResizedNormal(fileNamePath, NEWS_DEST_NORMAL_IMAGE_DIR_BASE, newsId);
                destImage.HDURL = destFileNameHD;
                destImage.NormalURL = destFileNameNormal;

                var imageId = dbContextService.Add<NewsImageInfo>(destImage);
                return (int)imageId;
            }
            return 0;
        }

        public NewsContentRefactor ImageSave(TouTiaoContent content, NewsContentRefactor result)
        {
            var imageList = content.ImageList;
            var HDURL = string.Empty;
            var NormalURL = string.Empty;
            if (imageList != null && imageList.Count > 0)
            {
                //NLogHelper.WriteInfo(string.Format("images count is {0}", imageList.Count));
                var newsId = content.NewsId;

                //Save first image
                SingleImageSave(imageList[0], newsId, out HDURL, out NormalURL);
                result.HDURL = HDURL;
                result.NormalURL = NormalURL;
            }
            return result;
        }

        public void SingleImageSave(TouTiaoImageInfo imageInfo, long newsId, out string HDURL, out string NormalURL)
        {
            HDURL = NormalURL = string.Empty;
            if (imageInfo.UrlList != null && imageInfo.UrlList.Count > 0)
            {
                //download single one from any one url
                var single_img_url = imageInfo.UrlList[0];
                MakeSureDIRExist(NEWS_IMAGE_DIR_BASE);
                MakeSureDIRExist(NEWS_DEST_HD_IMAGE_DIR_BASE);
                MakeSureDIRExist(NEWS_DEST_NORMAL_IMAGE_DIR_BASE);
                var fileNamePath = HttpHelper.DownloadFile(single_img_url, Path.Combine(NEWS_IMAGE_DIR_BASE, GetFileNameFromURL(single_img_url)));

                var destFileNameHD = ImageHelper.ResizedHD(fileNamePath, NEWS_DEST_HD_IMAGE_DIR_BASE, newsId);
                var destFileNameNormal = ImageHelper.ResizedNormal(fileNamePath, NEWS_DEST_NORMAL_IMAGE_DIR_BASE, newsId);
                HDURL = destFileNameHD;
                NormalURL = destFileNameNormal;
            }
        }

        private string GetFileNameFromURL(string uriPath)
        {
            Uri uri = new Uri(uriPath);
            return uri.AbsolutePath.Replace("/", "_");
        }

        private void MakeSureDIRExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private string GetDirFileName(string filePath)
        {
            var _fileInfo = new FileInfo(filePath);
            if (_fileInfo != null)
            {
                return _fileInfo.Name;
            }
            return string.Empty;
        }

        #endregion

        #endregion
    }
}
