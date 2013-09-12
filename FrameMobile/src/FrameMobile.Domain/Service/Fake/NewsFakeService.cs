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

        FetchTouTiaoService service;

        public string DIR_PREFIX = ConfigKeys.DEMO_TOUTIAO_FILE_PATH_ROOT.ConfigValue();

        public NewsFakeService()
        {
            service = new FetchTouTiaoService(dbContextService);
        }

        [ServiceCache]
        public IList<NewsSourceView> GetSourceList(MobileParam mobileParams)
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

            var result = sourcelist.To<IList<NewsSourceView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsLoadModeView> GetLoadModeList(MobileParam mobileParams)
        {
            #region instance
            var mode1 = new NewsLoadMode()
            {
                Id = 1,
                Name = "今日头条",
                NameLowCase = "toutiao",
                PackageName = "com.ss.android.article.news",
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var mode2 = new NewsLoadMode()
            {
                Id = 2,
                Name = "腾讯新闻",
                NameLowCase = "tentcent",
                PackageName = "com.tencent.news",
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var modelist = new List<NewsLoadMode>() { mode1, mode2 };

            var result = modelist.To<IList<NewsLoadModeView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams)
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

            var result = categorylist.To<IList<NewsCategoryView>>();

            return result;
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams)
        {
            #region instance
            var subcategory = new NewsSubCategory()
                {
                    Id = 1,
                    CategoryId = 1,
                    SourceId = 1,
                    Name = "news_hot",
                    DisplayName = "热门",
                    Cursor = 10,
                    Status = 1,
                    CreateDateTime = DateTime.Now
                };

            var subcategory2 = new NewsSubCategory()
            {
                Id = 2,
                CategoryId = 1,
                SourceId = 1,
                Name = "news_focus",
                DisplayName = "焦点",
                Cursor = 20,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var subcategory3 = new NewsSubCategory()
            {
                Id = 3,
                CategoryId = 2,
                SourceId = 1,
                Name = "news_finance",
                DisplayName = "财经郎眼",
                Cursor = 30,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var subcategory4 = new NewsSubCategory()
            {
                Id = 4,
                CategoryId = 2,
                SourceId = 1,
                Name = "news_today_finance",
                DisplayName = "今日财经",
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
        public IList<TouTiaoContentView> GetTouTiaoContentList(MobileParam mobileParams, int newsId, bool action, int categoryId, int startnum, int num, out int totalCount)
        {
            return GetTouTiaoContentViewList(categoryId, newsId, action, startnum, num, out totalCount);
        }

        #region Helper

        private List<TouTiaoContentModel> GetTouTiaoContentList(string response, int subcategoryId)
        {
            long cursor = 10;
            var result = new List<TouTiaoContentModel>();
            var dataResult = service.DeserializeTouTiao(response);

            var contentList = service.Anlynaze(dataResult, out cursor);
            var index = 1;
            foreach (var item in contentList)
            {
                var touTiaoModel = item.To<TouTiaoContentModel>();
                touTiaoModel.Id = index + (subcategoryId * 10);
                touTiaoModel.CategoryId = subcategoryId;
                result.Add(touTiaoModel);
                index++;
            }
            return result;

        }

        private List<TouTiaoContentView> GetTouTiaoContentViewList(List<TouTiaoContentModel> contentlist)
        {
            var result = contentlist.To<List<TouTiaoContentView>>();
            return result;
        }

        private List<TouTiaoContentView> GetTouTiaoContentViewList(int categoryId, int newsId, bool action, int startnum, int num, out int totalCont)
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
            switch (categoryId)
            {
                case 1:
                    var result1 = toutiaoresult1.Union(toutiaoresult2).ToList();
                    totalCont = result1.Count;
                    return result1.Skip(startnum - 1).Take(num).ToList();
                case 2:
                    var result2 = toutiaoresult3.Union(toutiaoresult4).ToList();
                    totalCont = result2.Count;
                    return result2.Skip(startnum - 1).Take(num).ToList();

                default:
                    return null;
            }
        }

        #endregion
    }
}
