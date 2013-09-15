using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;
using NCore;

namespace FrameMobile.Domain.Service
{
    public class NewsService : INewsService
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

        [ServiceCache]
        public IList<NewsSourceView> GetSourceList(MobileParam mobileParams)
        {
            var sourcelist = dbContextService.Find<NewsSource>(x => x.Status == 1);
            return sourcelist.To<IList<NewsSourceView>>();
        }

        [ServiceCache]
        public IList<NewsExtraAppView> GetExtraAppList(MobileParam mobileParams)
        {
            var extraAppList = dbContextService.Find<NewsExtraApp>(x => x.Status == 1);
            return extraAppList.To<IList<NewsExtraAppView>>();
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams)
        {
            var categorylist = dbContextService.Find<NewsCategory>(x => x.Status == 1);
            return categorylist.To<IList<NewsCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams)
        {
            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.Status == 1);
            return subcategorylist.To<IList<NewsSubCategoryView>>();
        }

        [ServiceCache]
        public IList<TouTiaoContentView> GetTouTiaoContentList(MobileParam mobileParams, int newsId, bool action, string categoryIds, int startnum, int num, out int totalCount)
        {
            var contentlist = new List<TouTiaoContentModel>();

            totalCount = 0;
            var categoryIdList = categoryIds.Split(';', '；').ToList();

            foreach (var item_category in categoryIdList)
            {
                var categoryId = item_category.ToInt32();

                contentlist = GetCategoryContentListByAction(categoryId, newsId, action);

                contentlist = contentlist.Union(contentlist).ToList();
            }
            totalCount = totalCount + contentlist.Count;
            var result = GetContentViewListByResolution(mobileParams, contentlist);
            return result.Skip(startnum - 1).Take(num).ToList();
        }

        private string GetImageURLByResloution(MobileParam mobileParams, long newsId)
        {
            if (string.IsNullOrEmpty(mobileParams.Resolution))
            {
                return string.Empty;
            }
            var lcdArray = mobileParams.Resolution.ToLower().Split('x');
            var width = lcdArray[0].ToInt32();
            var height = lcdArray[1].ToInt32();

            var newsImageInfo = dbContextService.Single<NewsImageInfo>(x => x.NewsId == newsId);
            if (newsImageInfo != null)
            {
                if (width > Const.NEWS_HD_RESOLUTION_WIDTH)
                    return newsImageInfo.HDURL;
                return newsImageInfo.NormalURL;
            }
            return string.Empty;
        }

        private List<TouTiaoContentModel> GetCategoryContentListByAction(int categoryId, int newsId, bool action)
        {
            var contentlist = new List<TouTiaoContentModel>();

            var endDateTime = DateTime.Now.AddDays(-10);
            if (action)
            {
                var subcategorycontentlist = dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == categoryId && x.Id > newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            else
            {
                var subcategorycontentlist = dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == categoryId && x.Id < newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            return contentlist;
        }

        private List<TouTiaoContentModel> GetSubCategoryContentListByAction(int subcategoryId, int newsId, bool action)
        {
            var contentlist = new List<TouTiaoContentModel>();

            var endDateTime = DateTime.Now.AddDays(-10);
            if (action)
            {
                var subcategorycontentlist = dbContextService.Find<TouTiaoContentModel>(x => x.SubCategoryId == subcategoryId && x.Id > newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            else
            {
                var subcategorycontentlist = dbContextService.Find<TouTiaoContentModel>(x => x.SubCategoryId == subcategoryId && x.Id < newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            return contentlist;
        }

        private IList<TouTiaoContentView> GetContentViewListByResolution(MobileParam mobileParams, List<TouTiaoContentModel> contentList)
        {
            var result = contentList.To<IList<TouTiaoContentView>>();

            if (result == null)
            {
                return null;
            }
            foreach (var item in result)
            {
                item.ImageURL = GetImageURLByResloution(mobileParams, item.NewsId);
            }
            return result;
        }
    }
}
