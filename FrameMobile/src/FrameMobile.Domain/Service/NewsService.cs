﻿using System;
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
    public class NewsService : NewsServiceBase, INewsService
    {
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
        public IList<NewsConfigView> GetConfigViewList(MobileParam mobileParams)
        {
            var configlist = dbContextService.Find<NewsConfig>(x => x.Status == 1);
            return configlist.To<IList<NewsConfigView>>();
        }

        [ServiceCache]
        public IList<NewsSourceView> GetSourceViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var sourcelist = new NewsSource().GetResultByVer<NewsSource>(cver, out sver);
            return sourcelist.To<IList<NewsSourceView>>();
        }

        [ServiceCache]
        public IList<NewsExtraAppView> GetExtraAppViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var extraAppList = new NewsExtraApp().GetResultByVer<NewsExtraApp>(cver, out sver);
            return extraAppList.To<IList<NewsExtraAppView>>();
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new NewsCategory().GetResultByVer<NewsCategory>(cver, out sver);
            return categorylist.To<IList<NewsCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams)
        {
            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.Status == 1);
            return subcategorylist.To<IList<NewsSubCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsContentView> GetNewsContentViewList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount)
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

        [ServiceCache]
        private List<NewsExtraApp> GetNewsExtraAppList()
        {
            var extraAppList = dbContextService.Find<NewsExtraApp>(x => x.Status == 1);

            return extraAppList.ToList();
        }

        [ServiceCache]
        private IEnumerable<NewsContentView> GetLocalContentViewList(List<NewsExtraApp> extraAppList, int imageType)
        {
            var contentlist = (from l in
                                   dbContextService.Find<NewsContent>(x => x.Rating > 0 && x.Status == 1)
                               orderby l.PublishTime descending, l.Rating descending
                               select new NewsContentView
                               {
                                   Id = l.Id,
                                   NewsId = l.NewsId,
                                   CategoryId = l.CategoryId,
                                   SubCategoryId = l.SubCategoryId,
                                   Site = l.Site,
                                   Title = l.Title,
                                   Summary = l.Summary,
                                   Content = l.Content == null ? string.Empty : l.Content,
                                   AppOpenURL = l.AppOpenURL,
                                   WAPURL = l.WAPURL,
                                   PublishTime = l.PublishTime,
                                   Stamp = l.PublishTime.UnixStamp(),
                                   ExtraAppId = l.ExtraAppId != 0 ? l.ExtraAppId : extraAppList.RandomInt(),
                                   ImageURL = l.NormalURL == null ? string.Empty : GetImageURLByType(l, imageType)
                               });
            return contentlist;
        }

        private List<NewsContentView> GetContentViewList(MobileParam mobileParams, List<int> categoryIds, long stamp, bool action)
        {
            var contentViewList = new List<NewsContentView>();

            var extraAppList = GetNewsExtraAppList();
            var imageType = GetImageURLTypeByResolution(mobileParams);

            var stampTime = stamp.UTCStamp();
            var endDateTime = stampTime.AddDays(-5);

            var categorycontentlist = action ? GetLatestNewsContentView(categoryIds, extraAppList, imageType, stampTime) :
                GetOldestNewsContentView(categoryIds, extraAppList, imageType, endDateTime, stampTime);

            var localContentList = GetLocalContentViewList(extraAppList, imageType);

            contentViewList = categorycontentlist.ToList();

            contentViewList = InsertRange(contentViewList, localContentList);

            return contentViewList;
        }

        private List<NewsContentView> InsertRange(List<NewsContentView> contentViewList, IEnumerable<NewsContentView> localContentList)
        {
            if (localContentList == null || localContentList.ToList().Count == 0)
            {
                return contentViewList;
            }

            var random = new Random(Guid.NewGuid().GetHashCode());
            var sum = random.Next(1, 3);

            var i = 0;
            foreach (var item in localContentList)
            {
                if (i > 20) break;

                for (int j = 0; j < sum; j++)
                {
                    var index = random.Next(1, 10);
                    contentViewList.Insert(10 * i + index, item);
                }
                i++;
            }
            return contentViewList;
        }

        private IEnumerable<NewsContentView> GetOldestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime endDateTime, DateTime stampTime)
        {
            var categorycontentlist = (from l in
                                           dbContextService.Find<NewsContent>(x => x.Status == 1 && x.PublishTime >= endDateTime && x.PublishTime <= stampTime)
                                       where categoryIds.Contains(l.CategoryId)
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
                                           Content = l.Content == null ? string.Empty : l.Content,
                                           AppOpenURL = l.AppOpenURL,
                                           WAPURL = l.WAPURL,
                                           PublishTime = l.PublishTime,
                                           Stamp = l.PublishTime.UnixStamp(),
                                           ExtraAppId = l.ExtraAppId != 0 ? l.ExtraAppId : extraAppList.RandomInt(),
                                           ImageURL = l.NormalURL == null ? string.Empty : GetImageURLByType(l, imageType)
                                       });
            return categorycontentlist;
        }

        private IEnumerable<NewsContentView> GetLatestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime stampTime)
        {
            var categorycontentlist = (from l in
                                           dbContextService.Find<NewsContent>(x => x.Status == 1 && x.PublishTime >= stampTime)
                                       where categoryIds.Contains(l.CategoryId)
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
                                           Content = l.Content == null ? string.Empty : l.Content,
                                           AppOpenURL = l.AppOpenURL,
                                           WAPURL = l.WAPURL,
                                           PublishTime = l.PublishTime,
                                           Stamp = l.PublishTime.UnixStamp(),
                                           ExtraAppId = l.ExtraAppId != 0 ? l.ExtraAppId : extraAppList.RandomInt(),
                                           ImageURL = l.NormalURL == null ? string.Empty : GetImageURLByType(l, imageType)
                                       });
            return categorycontentlist;
        }

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

        private int GetImageURLTypeByResolution(MobileParam mobileParams)
        {
            if (string.IsNullOrEmpty(mobileParams.Resolution))
            {
                return 0;
            }
            var width = mobileParams.Resolution.GetResolutionWidth();

            if (width > Const.NEWS_HD_RESOLUTION_WIDTH)
                return 2;
            return 1;
        }

        private string GetImageURLByType(NewsContent content, int imageType)
        {
            switch (imageType)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return content.NormalURL;
                case 2:
                    return content.HDURL;
            }
            return string.Empty;
        }

        #endregion
    }
}
