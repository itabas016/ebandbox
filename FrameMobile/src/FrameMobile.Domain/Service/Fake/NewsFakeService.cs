using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;
using NCore;

namespace FrameMobile.Domain.Service
{
    public class NewsFakeService : INewsService
    {
        private INewsDbContextService _dbContextService;
        public INewsDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        TouTiaoService service;

        public string DIR_PREFIX = ConfigKeys.DEMO_TOUTIAO_FILE_PATH_ROOT.ConfigValue();

        public string APK_DOWNLOAD_PREFIX_URL = ConfigKeys.TYD_NEWS_APP_DOWNLOAD_PREFIX_URL.ConfigValue();

        public NewsFakeService()
        {
            service = new TouTiaoService(dbContextService);
        }

        public string TimeConvert(string timeformat, long stamp)
        {
            return null;
        }

        [ServiceCache]
        public IList<NewsConfigView> GetConfigViewList(MobileParam mobileParams)
        {
            #region instance
            var config = new NewsConfig()
                {
                    Id = 1,
                    Name = "提供商",
                    NameLowCase = "newssource",
                    Version = 1,
                    Status = 1,
                    CreateDateTime = DateTime.Now
                };

            var config2 = new NewsConfig()
            {
                Id = 2,
                Name = "分类",
                NameLowCase = "newscategory",
                Version = 2,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var config3 = new NewsConfig()
            {
                Id = 3,
                Name = "外推应用",
                NameLowCase = "newsextraapp",
                Version = 1,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var configlist = new List<NewsConfig>() { config, config2, config3 };

            var result = configlist.To<IList<NewsConfigView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsSourceView> GetSourceViewList(MobileParam mobileParams, int cver, out int sver)
        {
            #region instance
            var source = new NewsSource()
            {
                Id = 1,
                Name = "今日头条",
                NameLowCase = "toutiao",
                PackageName = "com.ss.android.article.news",
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var source2 = new NewsSource()
            {
                Id = 2,
                Name = "腾讯新闻",
                NameLowCase = "tentcent",
                PackageName = "com.tencent.news",
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var sourcelist = new List<NewsSource>() { source, source2 };
            sver = 1;
            var result = sourcelist.To<IList<NewsSourceView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsExtraAppView> GetExtraAppViewList(MobileParam mobileParams, int cver, out int sver)
        {
            #region instance

            var pkgName_TouTiao = "com.ss.android.article.news";
            var pkgName_QQBrower = " com.tencent.mtt";
            var mode1 = new NewsExtraApp()
            {
                Id = 1,
                Name = "今日头条",
                NameLowCase = "toutiao",
                PackageName = pkgName_TouTiao,
                ExtraLinkUrl = string.Format(APK_DOWNLOAD_PREFIX_URL, pkgName_TouTiao),
                IsBrower = 0,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var mode2 = new NewsExtraApp()
            {
                Id = 2,
                Name = "QQ浏览器",
                NameLowCase = "tentcent",
                PackageName = pkgName_QQBrower,
                ExtraLinkUrl = string.Format(APK_DOWNLOAD_PREFIX_URL, pkgName_QQBrower),
                IsBrower = 1,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var modelist = new List<NewsExtraApp>() { mode1, mode2 };
            sver = 1;
            var result = modelist.To<IList<NewsExtraAppView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsInfAddressView> GetInfAddressViewList(MobileParam mobileParams, int cver, out int sver)
        {
            #region instance

            var mode1 = new NewsInfAddress()
            {
                Id = 1,
                Name = "腾讯热门",
                SourceId = 2,
                CategoryId = 1,
                SubCategoryId = 0,
                IsStamp = 0,
                InfAddress = "http://openapi.inews.qq.com/getNewsByChlidVerify?chlid=news&refer=openapi_for_tianyida&appkey=3XfMefMGRHJMpKZHKbKxFWvsFgO4FV&n=10",
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var mode2 = new NewsInfAddress()
            {
                Id = 2,
                Name = "腾讯科技",
                SourceId = 2,
                CategoryId = 2,
                SubCategoryId = 0,
                IsStamp = 0,
                InfAddress = "http://openapi.inews.qq.com/getNewsByChlidVerify?chlid=tech&refer=openapi_for_tianyida&appkey=3XfMefMGRHJMpKZHKbKxFWvsFgO4FV&n=10",
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var modelist = new List<NewsInfAddress>() { mode1, mode2 };
            sver = 1;
            var result = modelist.To<IList<NewsInfAddressView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            #region instance
            var category = new NewsCategory()
                {
                    Id = 1,
                    Name = "热点",
                    Status = 1,
                    CreateDateTime = DateTime.Now
                };
            var category2 = new NewsCategory()
            {
                Id = 2,
                Name = "财经",
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var categorylist = new List<NewsCategory>() { category, category2 };
            sver = 1;
            var result = categorylist.To<IList<NewsCategoryView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams)
        {
            #region instance
            var subcategory = new NewsSubCategory()
                {
                    Id = 1,
                    CategoryId = 1,
                    SourceId = 1,
                    NameLowCase = "news_hot",
                    Name = "热门",
                    Cursor = 10,
                    Status = 1,
                    CreateDateTime = DateTime.Now
                };

            var subcategory2 = new NewsSubCategory()
            {
                Id = 2,
                CategoryId = 1,
                SourceId = 1,
                NameLowCase = "news_focus",
                Name = "焦点",
                Cursor = 20,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var subcategory3 = new NewsSubCategory()
            {
                Id = 3,
                CategoryId = 2,
                SourceId = 1,
                NameLowCase = "news_finance",
                Name = "财经郎眼",
                Cursor = 30,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var subcategory4 = new NewsSubCategory()
            {
                Id = 4,
                CategoryId = 2,
                SourceId = 1,
                NameLowCase = "news_today_finance",
                Name = "今日财经",
                Cursor = 40,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var subcategorylist = new List<NewsSubCategory>() { subcategory, subcategory2, subcategory3, subcategory4 };

            var restult = subcategorylist.To<IList<NewsSubCategoryView>>();

            return restult;
        }

        [ServiceCache]
        public IList<NewsRadarView> GetNewsRadarViewList(MobileParam mobileParams, int cver, out int sver)
        {
            #region SubNewsRadar
            var newssubradar = new NewsRadarElementView
            {
                Id = 1,
                Name = "团购",
                Status = 1
            };
            var newssubradar2 = new NewsRadarElementView
            {
                Id = 2,
                Name = "美食",
                Status = 1
            };
            var newssubradar3 = new NewsRadarElementView
            {
                Id = 3,
                Name = "酒店",
                Status = 1
            };
            var newssubradar4 = new NewsRadarElementView
            {
                Id = 4,
                Name = "公交站",
                Status = 1
            };
            var newssubradar5 = new NewsRadarElementView
            {
                Id = 5,
                Name = "电影院",
                Status = 1
            };
            var newssubradar6 = new NewsRadarElementView
            {
                Id = 6,
                Name = "酒吧",
                Status = 1
            };
            var newssubradar7 = new NewsRadarElementView
            {
                Id = 7,
                Name = "网吧",
                Status = 1
            };
            var newssubradar8 = new NewsRadarElementView
            {
                Id = 8,
                Name = "洗浴",
                Status = 1
            };
            var newssubradar9 = new NewsRadarElementView
            {
                Id = 9,
                Name = "丽人",
                Status = 1
            };
            var newssubradar10 = new NewsRadarElementView
            {
                Id = 10,
                Name = "ATM",
                Status = 1
            };
            var newssubradar11 = new NewsRadarElementView
            {
                Id = 11,
                Name = "邮局",
                Status = 1
            };
            #endregion

            #region instance
            var newsradar = new NewsRadarView()
            {
                Id = 1,
                Name = "热门",
                Status = 1,
                NewsRadarElementList = new List<NewsRadarElementView>
                {
                    newssubradar,newssubradar2,newssubradar3,newssubradar4,newssubradar5,newssubradar6
                }
            };

            var newsradar2 = new NewsRadarView()
            {
                Id = 2,
                Name = "休闲娱乐",
                Status = 1,
                NewsRadarElementList = new List<NewsRadarElementView>
                {
                    newssubradar6,newssubradar7,newssubradar8,newssubradar9
                }
            };

            var newsradar3 = new NewsRadarView()
            {
                Id = 3,
                Name = "生活服务",
                Status = 1,
                NewsRadarElementList = new List<NewsRadarElementView>
                {
                    newssubradar10,newssubradar11
                }
            };

            #endregion

            var newsradarlist = new List<NewsRadarView>() { newsradar, newsradar2, newsradar3 };
            sver = 1;
            return newsradarlist;
        }

        [ServiceCache]
        public IList<NewsContentView> GetNewsContentViewList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount)
        {
            return GetTouTiaoContentViewList(mobileParams, categoryIds, stamp, action, startnum, num, out totalCount);
        }

        #region Helper

        private List<NewsContent> GetTouTiaoContentList(string response, int subcategoryId)
        {
            long cursor = 10;
            var result = new List<NewsContent>();
            var dataResult = service.DeserializeTouTiao(response);

            var contentList = service.Anlynaze(dataResult, out cursor);
            var index = 1;
            foreach (var item in contentList)
            {
                var touTiaoModel = item.To<NewsContent>();
                touTiaoModel.Id = index + (subcategoryId * 10);
                touTiaoModel.CategoryId = subcategoryId;
                result.Add(touTiaoModel);
                index++;
            }
            return result;

        }

        private List<NewsContentView> GetTouTiaoContentViewList(List<NewsContent> contentlist)
        {
            var result = contentlist.To<List<NewsContentView>>();
            return result;
        }

        private List<NewsContentView> GetTouTiaoContentViewList(MobileParam mobileParams, string categoryIds, long stamp, bool action, int startnum, int num, out int totalCont)
        {
            var response1 = string.Empty;
            var response2 = string.Empty;
            var response3 = string.Empty;
            var response4 = string.Empty;

            using (var sr = new StreamReader(string.Format("{0}{1}", DIR_PREFIX, "\\TouTiaoContent_hot.txt"))) { response1 = sr.ReadToEnd(); }

            using (var sr = new StreamReader(string.Format("{0}{1}", DIR_PREFIX, "\\TouTiaoContent_focus.txt"))) { response2 = sr.ReadToEnd(); }

            using (var sr = new StreamReader(string.Format("{0}{1}", DIR_PREFIX, "\\TouTiaoContent_finance.txt"))) { response3 = sr.ReadToEnd(); }

            using (var sr = new StreamReader(string.Format("{0}{1}", DIR_PREFIX, "\\TouTiaoContent_today_finance.txt"))) { response4 = sr.ReadToEnd(); }

            var toutiaoresult1 = GetTouTiaoContentViewList(GetTouTiaoContentList(response1, 1));
            var toutiaoresult2 = GetTouTiaoContentViewList(GetTouTiaoContentList(response2, 2));
            var toutiaoresult3 = GetTouTiaoContentViewList(GetTouTiaoContentList(response3, 3));
            var toutiaoresult4 = GetTouTiaoContentViewList(GetTouTiaoContentList(response4, 4));

            totalCont = 0;

            var categoryList = categoryIds.Split(';', '；').ToList();
            var result = new List<NewsContentView>();
            foreach (var categoryId in categoryList)
            {
                switch (categoryId.ToInt32())
                {
                    case 1:
                        result = toutiaoresult1.Union(toutiaoresult2).ToList();
                        break;
                    case 2:
                        result = toutiaoresult3.Union(toutiaoresult4).ToList();
                        break;
                    default: break;
                }
                result = result.Union(result).ToList();
                totalCont = totalCont + result.Count;
            }

            foreach (var item in result)
            {
                var r = new Random();
                item.ImageURL = GetFakeImageURLByResolution(mobileParams);
                item.ExtraAppId = r.Next(1, 3);
            }
            return result.Skip(startnum - 1).Take(num).ToList();
        }

        private string GetFakeImageURLByResolution(MobileParam mobileParams)
        {
            var HD_ImageURL = string.Format("{0}/720/2750762044_origin_283_6383591116.jpg", ConfigKeys.TYD_NEWS_IMAGE_FILE_URL.ConfigValue());
            var Normal_ImageURL = string.Format("{0}/480/2748447810_origin_281_8233220044.jpg", ConfigKeys.TYD_NEWS_IMAGE_FILE_URL.ConfigValue());
            var resolutionArray = mobileParams.Resolution.ToLower().Split('x');
            var width = resolutionArray[0].ToInt32();
            var height = resolutionArray[1].ToInt32();
            if (width > 720)
            {
                return HD_ImageURL;
            }
            return Normal_ImageURL;
        }

        #endregion
    }
}
