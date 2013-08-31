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

namespace FrameMobile.Domain.Service
{
    public class FetchTouTiaoService
    {
        #region Prop

        private IDataBaseService _dataBaseService;
        public IDataBaseService DataBaseService
        {
            get
            {
                if (_dataBaseService == null)
                {
                    _dataBaseService = ObjectFactory.GetInstance<IDataBaseService>();
                }
                return _dataBaseService;
            }
            set
            {
                _dataBaseService = value;
            }
        }

        public static string NEWS_RESOURCES_DIR_ROOT = ConfigKeys.TYD_NEWS_RESOURCES_DIR_ROOT.ConfigValue();

        public string NEWS_IMAGE_DIR_BASE = string.Format("{0}\\Images", NEWS_RESOURCES_DIR_ROOT);

        public string NEWS_IMAGE_FILE_URL = ConfigKeys.TYD_NEWS_IMAGE_FILE_URL.ConfigValue();

        public const string NEWS_SOURCES = "TouTiao";

        #endregion

        #region Ctor
        public FetchTouTiaoService(IDataBaseService dataBaseService)
        {
            this.DataBaseService = dataBaseService;
        }
        #endregion

        #region Method

        public TouTiaoParameter GenerateParam()
        {
            var param = new TouTiaoParameter();
            return param;
        }

        public string Request(string category, long cursor = 0, int count = 20)
        {
            var param = GenerateParam();
            var request_url = ConfigKeys.TYD_NEWS_TOUTIAO_REQUEST_URL.ConfigValue();

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
            var cursor = GetCategoryCursor(category);
            var response = Request(category);
            var instance = DeserializeTouTiao(response);
            var contentList = Anlynaze(instance, out cursor);

            foreach (var item_content in contentList)
            {
                var touTiaoModel = item_content.To<TouTiaoModel>();
                touTiaoModel.CategoryId = GetSubCategoryId(category);
                DataBaseService.Add<TouTiaoModel>(touTiaoModel);
                ImageListSave(item_content);
            }
            return cursor;
        }

        public void SingleCapture(string categoryName)
        {
            var cursor = GetCurrentCursor(categoryName);
            UpdateCategoryCursor(categoryName,cursor);
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
                LogHelper.WriteError(ex.Message);
                LogHelper.WriteError(ex.StackTrace);
            }
        }

        #region Helper

        public void UpdateCategoryCursor(string categoryName, long cursor)
        {
            var subcategory = DataBaseService.Single<NewsSubCategory>(x => x.Name == categoryName);
            if (cursor != subcategory.Cursor)
            {
                subcategory.Cursor = cursor;
                DataBaseService.Update<NewsSubCategory>(subcategory);
            }
        }

        public long GetCategoryCursor(string categoryName)
        {
            var subcategory = DataBaseService.Single<NewsSubCategory>(x => x.Name == categoryName);

            if (subcategory != null)
            {
                return subcategory.Cursor;
            }
            return 0;
        }

        public int GetSubCategoryId(string categoryName)
        {
            var subCategory = DataBaseService.Single<NewsSubCategory>(x => x.Name == categoryName);
            if (subCategory == null)
            {
                var newsSubCategory = new NewsSubCategory();

                newsSubCategory.SourceId = GetSourceId();

                newsSubCategory = MatchCategory(newsSubCategory, categoryName);

                DataBaseService.Add<NewsSubCategory>(newsSubCategory);

                return newsSubCategory.Id;
            }
            return subCategory.Id;
        }

        public int GetSourceId()
        {
            var source = DataBaseService.Single<NewsSource>(x => x.Name == NEWS_SOURCES);
            if (source == null)
            {
                DataBaseService.Add<NewsSource>(new NewsSource { Name = NEWS_SOURCES });
                return source.Id;
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

        public void ImageListSave(TouTiaoContent content)
        {
            var imageList = content.ImageList;
            if (imageList == null && imageList.Count > 0)
            {
                var newsId = content.Id;
                foreach (var item in imageList)
                {
                    SingleImageSave(item, newsId);
                }
            }
        }

        public void SingleImageSave(TouTiaoImageInfo imageInfo, long newsId)
        {
            var destImage = imageInfo.To<NewsImageInfo>();
            destImage.NewsId = newsId;
            if (imageInfo.UrlList != null && imageInfo.UrlList.Count > 0)
            {
                foreach (var item in imageInfo.UrlList)
                {
                    HttpHelper.DownloadFile(item, Path.Combine(NEWS_IMAGE_DIR_BASE, GetFileNameFromURL(item)));
                    destImage.URL = string.Format("{0}/{1}", NEWS_IMAGE_FILE_URL, GetFileNameFromURL(item));
                    DataBaseService.Add<NewsImageInfo>(destImage);
                }
            }
        }

        private string GetFileNameFromURL(string uriPath)
        {
            //http://p0.pstatp.com/origin/252/6926772543
            Uri uri = new Uri(uriPath);
            return uri.AbsolutePath.Replace("/", "_");
        }

        #endregion

        #endregion
    }
}
