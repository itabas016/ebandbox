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
            var contentlist = new List<TouTiaoContentView>();

            totalCount = 0;
            var categoryIdList = categoryIds.Split(';', '；').ToList();

            foreach (var item_category in categoryIdList)
            {
                var categoryId = item_category.ToInt32();

                //contentlist = GetCategoryContentListByAction(categoryId, newsId, action);

                contentlist = GetContentViewList(mobileParams, categoryId, newsId, action);
                contentlist = contentlist.Union(contentlist).ToList();
            }
            totalCount = totalCount + contentlist.Count;
            //var result = GetCompleteContentViewList(mobileParams, contentlist);
            return contentlist.Skip(startnum - 1).Take(num).ToList();
        }

        #region Helper

        //1 为Normal 2为HD
        private int GetImageURLTypeByResolution(MobileParam mobileParams)
        {
            if (string.IsNullOrEmpty(mobileParams.Resolution))
            {
                return 0;
            }
            var lcdArray = mobileParams.Resolution.ToLower().Split('x');
            var width = lcdArray[0].ToInt32();
            var height = lcdArray[1].ToInt32();

            if (width > Const.NEWS_HD_RESOLUTION_WIDTH)
                return 2;
            return 1;
        }

        private string GetImageURLByType(NewsImageInfo image, int imageType)
        {
            switch (imageType)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return image.NormalURL;
                case 2:
                    return image.HDURL;
            }
            return string.Empty;
        }

        private List<NewsExtraApp> GetNewsExtraAppList()
        {
            var extraAppList = dbContextService.Find<NewsExtraApp>(x => x.Status == 1);

            return extraAppList.ToList();
        }

        private List<TouTiaoContentView> GetContentViewList(MobileParam mobileParams, int categoryId, int newsId, bool action)
        {
            var contentViewList = new List<TouTiaoContentView>();

            var extraAppList = GetNewsExtraAppList();
            var imageType = GetImageURLTypeByResolution(mobileParams);

            var endDateTime = DateTime.Now.AddDays(-10);
            if (action)
            {
                var subcategorycontentlist = (from l in
                                                  dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == categoryId
                                                      && x.Id > newsId && x.Status == 1 && x.PublishTime > endDateTime)
                                              join m in
                                                  dbContextService.Find<NewsImageInfo>(y => y.Status == 1)
                                              on l.NewsId equals (m.NewsId)
                                              orderby l.PublishTime descending
                                              select new TouTiaoContentView
                                              {
                                                  Id = l.Id,
                                                  NewsId = l.NewsId,
                                                  CategoryId = l.CategoryId,
                                                  SubCategoryId = l.SubCategoryId,
                                                  Site = l.Site,
                                                  Title = l.Title,
                                                  Summary = l.Summary,
                                                  Content = l.Content,
                                                  AppOpenURL = l.AppOpenURL,
                                                  WAPURL = l.WAPURL,
                                                  PublishTime = l.PublishTime,
                                                  ExtraAppId = extraAppList.RandomInt(),
                                                  ImageURL = GetImageURLByType(m, imageType)
                                              });
                contentViewList = contentViewList.Union(subcategorycontentlist).ToList();
            }
            else
            {
                var subcategorycontentlist = (from l in
                                                  dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == categoryId
                                                      && x.Id < newsId && x.Status == 1 && x.PublishTime > endDateTime)
                                              join m in
                                                  dbContextService.Find<NewsImageInfo>(y => y.Status == 1)
                                              on l.NewsId equals m.NewsId
                                              orderby l.PublishTime descending
                                              select new TouTiaoContentView
                                              {
                                                  Id = l.Id,
                                                  NewsId = l.NewsId,
                                                  CategoryId = l.CategoryId,
                                                  SubCategoryId = l.SubCategoryId,
                                                  Site = l.Site,
                                                  Title = l.Title,
                                                  Summary = l.Summary,
                                                  Content = l.Content,
                                                  AppOpenURL = l.AppOpenURL,
                                                  WAPURL = l.WAPURL,
                                                  PublishTime = l.PublishTime,
                                                  ExtraAppId = extraAppList.RandomInt(),
                                                  ImageURL = GetImageURLByType(m, imageType)
                                              });
                contentViewList = contentViewList.Union(subcategorycontentlist).ToList();
            }
            return contentViewList;
        }

        #region Old Method Helper

        private string GetImageURLByResolution(MobileParam mobileParams, long newsId)
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
                var subcategorycontentlist = (from l in dbContextService.Find<TouTiaoContentModel>(x => x.CategoryId == categoryId && x.Id > newsId && x.Status == 1 && x.PublishTime > endDateTime) join m in dbContextService.Find<NewsImageInfo>(y => y.Status == 1) on l.NewsId equals (m.NewsId) orderby l.PublishTime descending select l);
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

        private IList<TouTiaoContentView> GetCompleteContentViewList(MobileParam mobileParams, List<TouTiaoContentModel> contentList)
        {
            var result = contentList.To<IList<TouTiaoContentView>>();
            var extraAppList = GetNewsExtraAppList();
            if (result == null)
            {
                return null;
            }
            foreach (var item in result)
            {
                item.ExtraAppId = extraAppList.RandomInt();
                item.ImageURL = GetImageURLByResolution(mobileParams, item.NewsId);
            }
            return result;
        }

        #endregion

        #endregion
    }
}
