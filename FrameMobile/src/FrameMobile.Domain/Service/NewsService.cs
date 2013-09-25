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

        public string TimeConvert(string timeformat, long stamp)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(timeformat))
            {
                sb.Append(timeformat.ToExactDateTime(DateTimeFormat.COMMON_TO_SECOND).UnixStamp());
            }
            else if (stamp > 0)
            {
                sb.Append(stamp.UTCStamp());
            }
            else if (!string.IsNullOrEmpty(timeformat) != null && stamp > 0)
            {
                sb.Append(timeformat.ToExactDateTime(DateTimeFormat.COMMON_TO_SECOND).UnixStamp());
                sb.AppendLine();
                sb.Append(stamp.UTCStamp());
            }
            else
            {
                sb.Append(DateTime.Now.UnixStamp());
                sb.AppendLine();
                sb.Append(DateTime.Now);
            }

            return sb.ToString();
        }

        [ServiceCache]
        public IList<NewsConfigView> GetConfigList(MobileParam mobileParams)
        {
            var configlist = dbContextService.Find<NewsConfig>(x => x.Status == 1);
            return configlist.To<IList<NewsConfigView>>();
        }

        [ServiceCache]
        public IList<NewsSourceView> GetSourceList(MobileParam mobileParams, int cver, out int sver)
        {
            var sourcelist = new NewsSource().GetResultByVer<NewsSource>(cver, out sver);
            return sourcelist.To<IList<NewsSourceView>>();
        }

        [ServiceCache]
        public IList<NewsExtraAppView> GetExtraAppList(MobileParam mobileParams, int cver, out int sver)
        {
            var extraAppList = new NewsExtraApp().GetResultByVer<NewsExtraApp>(cver, out sver);
            return extraAppList.To<IList<NewsExtraAppView>>();
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new NewsCategory().GetResultByVer<NewsCategory>(cver, out sver);
            return categorylist.To<IList<NewsCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryList(MobileParam mobileParams)
        {
            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.Status == 1);
            return subcategorylist.To<IList<NewsSubCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsContentView> GetTouTiaoContentList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount)
        {
            var contentlist = new List<NewsContentView>();

            totalCount = 0;
            var categoryIdList = categoryIds.Split(';', '；').ToList().ToInt32List();

            contentlist = GetContentViewList(mobileParams, categoryIdList, stamp, action);

            totalCount = contentlist.Count;

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

        private List<NewsContentView> GetContentViewList(MobileParam mobileParams, List<int> categoryIds, long stamp, bool action)
        {
            var contentViewList = new List<NewsContentView>();

            var extraAppList = GetNewsExtraAppList();
            var imageType = GetImageURLTypeByResolution(mobileParams);

            var endDateTime = DateTime.Now.AddDays(-5);
            var stampTime = stamp.UTCStamp();

            var categorycontentlist = action ? GetLatestNewsContentView(categoryIds, extraAppList, imageType, stampTime) :
                GetOldestNewsContentView(categoryIds, extraAppList, imageType, endDateTime, stampTime);

            contentViewList = categorycontentlist.ToList();

            return contentViewList;
        }

        private IEnumerable<NewsContentView> GetOldestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime endDateTime, DateTime stampTime)
        {
            var categorycontentlist = (from l in
                                           dbContextService.Find<NewsContent>(x => x.Status == 1 && x.PublishTime > endDateTime && x.PublishTime < stampTime)
                                       join m in
                                           dbContextService.Find<NewsImageInfo>(y => y.Status == 1)
                                       on l.NewsId equals m.NewsId
                                       into d
                                       where categoryIds.Contains(l.CategoryId)
                                       from s in d.DefaultIfEmpty()
                                       orderby l.PublishTime descending
                                       select new NewsContentView
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
                                           Stamp = l.PublishTime.UnixStamp(),
                                           ExtraAppId = l.ExtraAppId != 0 ? l.ExtraAppId : extraAppList.RandomInt(),
                                           ImageURL = s == null ? string.Empty : GetImageURLByType(s, imageType)
                                       });
            return categorycontentlist;
        }

        private IEnumerable<NewsContentView> GetLatestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime stampTime)
        {
            var categorycontentlist = (from l in
                                           dbContextService.Find<NewsContent>(x => x.Status == 1 && x.PublishTime > stampTime)
                                       join m in
                                           dbContextService.Find<NewsImageInfo>(y => y.Status == 1)
                                       on l.NewsId equals (m.NewsId)
                                       into d
                                       where categoryIds.Contains(l.CategoryId)
                                       from s in d.DefaultIfEmpty()
                                       orderby l.PublishTime descending
                                       select new NewsContentView
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
                                           Stamp = l.PublishTime.UnixStamp(),
                                           ExtraAppId = l.ExtraAppId != 0 ? l.ExtraAppId : extraAppList.RandomInt(),
                                           ImageURL = s == null ? string.Empty : GetImageURLByType(s, imageType)
                                       });
            return categorycontentlist;
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

        private List<NewsContent> GetCategoryContentListByAction(int categoryId, int newsId, bool action)
        {
            var contentlist = new List<NewsContent>();

            var endDateTime = DateTime.Now.AddDays(-10);
            if (action)
            {
                var subcategorycontentlist = (from l in dbContextService.Find<NewsContent>(x => x.CategoryId == categoryId && x.Id > newsId && x.Status == 1 && x.PublishTime > endDateTime) join m in dbContextService.Find<NewsImageInfo>(y => y.Status == 1) on l.NewsId equals (m.NewsId) orderby l.PublishTime descending select l);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            else
            {
                var subcategorycontentlist = dbContextService.Find<NewsContent>(x => x.CategoryId == categoryId && x.Id < newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            return contentlist;
        }

        private List<NewsContent> GetSubCategoryContentListByAction(int subcategoryId, int newsId, bool action)
        {
            var contentlist = new List<NewsContent>();

            var endDateTime = DateTime.Now.AddDays(-10);
            if (action)
            {
                var subcategorycontentlist = dbContextService.Find<NewsContent>(x => x.SubCategoryId == subcategoryId && x.Id > newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            else
            {
                var subcategorycontentlist = dbContextService.Find<NewsContent>(x => x.SubCategoryId == subcategoryId && x.Id < newsId && x.Status == 1 && x.PublishTime > endDateTime).OrderByDescending(x => x
                    .PublishTime);
                contentlist = contentlist.Union(subcategorycontentlist).ToList();
            }
            return contentlist;
        }

        private IList<NewsContentView> GetCompleteContentViewList(MobileParam mobileParams, List<NewsContent> contentList)
        {
            var result = contentList.To<IList<NewsContentView>>();
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
