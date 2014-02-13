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
using FrameMobile.Model.Radar;

namespace FrameMobile.Domain.Service
{
    public class NewsService : NewsDbContextService, INewsService
    {
        [ServiceCache]
        public IList<NewsConfigView> GetConfigViewList(MobileParam mobileParams)
        {
            var configlist = dbContextService.Find<NewsConfig>(x => x.Status == 1);
            return configlist.To<IList<NewsConfigView>>();
        }

        [ServiceCache]
        public IList<NewsSourceView> GetSourceViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var sourcelist = new NewsSource().ReturnNewsInstance<NewsSource>(cver, out sver);
            return sourcelist.To<IList<NewsSourceView>>();
        }

        [ServiceCache]
        public IList<NewsExtraAppView> GetExtraAppViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var extraAppList = new NewsExtraApp().ReturnNewsInstance<NewsExtraApp>(cver, out sver);
            return extraAppList.To<IList<NewsExtraAppView>>();
        }

        [ServiceCache]
        public IList<NewsInfAddressView> GetInfAddressViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var infAddressList = new NewsInfAddress().ReturnNewsInstance<NewsInfAddress>(cver, out sver);
            return infAddressList.To<IList<NewsInfAddressView>>();
        }

        [ServiceCache]
        public IList<NewsCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new NewsCategory().ReturnNewsInstance<NewsCategory>(cver, out sver);
            return categorylist.To<IList<NewsCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams)
        {
            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.Status == 1);
            return subcategorylist.To<IList<NewsSubCategoryView>>();
        }

        [ServiceCache]
        public IList<NewsRadarView> GetNewsRadarViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var radarlist = new RadarCategory().ReturnRadarInstance<RadarCategory>(cver, out sver);
            var subradarlist = new RadarElement().ReturnRadarInstance<RadarElement>(cver, out sver);

            return ConvertByRadar(radarlist, subradarlist);
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

        private List<NewsContentView> GetContentViewList(MobileParam mobileParams, List<int> categoryIds, long stamp, bool action)
        {
            var contentViewList = new List<NewsContentView>();

            var extraAppList = GetNewsExtraAppList();
            var imageType = GetImageURLTypeByResolution(mobileParams);

            var stampTime = stamp.UTCStamp();
            var endDateTime = stampTime.AddDays(-5);

            switch (action)
            {
                case false:
                    contentViewList = GetOldestNewsContentView(categoryIds, extraAppList, imageType, endDateTime, stampTime).ToList();
                    break;
                default:
                    contentViewList = GetLatestNewsContentView(categoryIds, extraAppList, imageType, stampTime).ToList();
                    break;
            }

            var localContentList = GetLocalContentViewList(extraAppList, imageType);

            contentViewList = InsertRange(contentViewList, localContentList);

            return contentViewList;
        }

        [ServiceCache]
        private List<NewsExtraApp> GetNewsExtraAppList()
        {
            var extraAppList = dbContextService.Find<NewsExtraApp>(x => x.Status == 1);

            return extraAppList.ToList();
        }

        [ServiceCache]
        private int GetImageURLTypeByResolution(MobileParam mobileParams)
        {
            var resolution = mobileParams.Resolution;

            if (string.IsNullOrEmpty(resolution))
            {
                return 0;
            }
            var width = resolution.GetResolutionWidth();

            if (width > Const.NEWS_HD_RESOLUTION_WIDTH)
                return 2;
            return 1;
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
                                       orderby l.PublishTime ascending
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

        private List<NewsContentView> InsertRange(List<NewsContentView> contentViewList, IEnumerable<NewsContentView> localContentList)
        {
            var count_total = contentViewList.Count;
            var count_local = localContentList.ToList().Count;

            if (count_total == 0 || localContentList == null || count_local == 0)
            {
                return contentViewList;
            }

            var random = new Random(Guid.NewGuid().GetHashCode());
            var sum_range = count_total / 10 >= 4 ? 4 : count_total / 10 + 1;

            var sum = random.Next(1, sum_range);

            var i = 0;
            foreach (var item in localContentList)
            {
                if (i > (count_local / sum + 1) * 10) break;

                for (int j = 0; j < sum; j++)
                {
                    var index = random.Next(1, 10);
                    index = 10 * i + index;

                    if (index > count_total)
                    {
                        index = count_total;
                    }
                    contentViewList.Insert(index, item);
                }

                i++;
            }
            return contentViewList;
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

        private IList<NewsRadarView> ConvertByRadar(IList<RadarCategory> radarlist, IList<RadarElement> subradarlist)
        {
            var radarviewlist = radarlist.To<IList<NewsRadarView>>();
            var subradarviewlist = subradarlist.To<IList<NewsRadarElementView>>();

            foreach (var item in radarviewlist)
            {
                item.NewsRadarElementList = subradarviewlist.Where(x => x.RadarCategoryIds.Contains(item.Id)).ToList();
            }
            return radarviewlist;
        }

        #endregion
    }
}
