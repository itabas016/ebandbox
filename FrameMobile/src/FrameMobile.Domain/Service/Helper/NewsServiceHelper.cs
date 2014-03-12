using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;
using NCore;

namespace FrameMobile.Domain.Service
{
    public class NewsServiceHelper : NewsDbContextService, INewsServiceHelper
    {
        public INewsService NewsService
        {
            get
            {
                if (_newsService == null)
                    _newsService = ObjectFactory.GetInstance<INewsService>();

                return _newsService;
            }
            set
            {
                _newsService = value;
            }
        }
        private INewsService _newsService;

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsExtraApp> GetNewsExtraAppList()
        {
            var extraAppList = dbContextService.Find<NewsExtraApp>(x => x.Status == 1);

            return extraAppList;
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public int GetImageURLTypeByResolution(MobileParam mobileParams)
        {
            var resolution = mobileParams.Resolution;

            if (string.IsNullOrEmpty(resolution))
            {
                return 0;
            }
            var width = resolution.GetResolutionWidth();

            if (width >= Const.NEWS_HD_RESOLUTION_WIDTH)
                return 2;
            return 1;
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public int GetExtraRatioByChannel(MobileParam mobileParams)
        {
            var channel = mobileParams.Channel;
            var ratio = ConfigKeys.TYD_AD_EXTRA_RATIO_DEFAULT_VALUE.ConfigValue().ToInt32();

            if (!string.IsNullOrEmpty(channel))
            {
                var mobilechannel = MobileUIService.GetMobileChannel(channel);
                if (mobilechannel != null)
                {
                    var extraratio = dbContextService.Single<NewsExtraRatio>(x => x.Status == 1 && x.ChannelId == mobilechannel.Id).MakeSureNotNull() as NewsExtraRatio;
                    ratio = extraratio.Ratio;
                }
            }
            return ratio;
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IEnumerable<NewsContentView> GetLocalContentViewList(List<NewsExtraApp> extraAppList, int imageType)
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

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IEnumerable<NewsContentView> GetOldestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime endDateTime, DateTime stampTime)
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

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IEnumerable<NewsContentView> GetLatestNewsContentView(List<int> categoryIds, List<NewsExtraApp> extraAppList, int imageType, DateTime stampTime)
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

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public NewsExtraResult GetNewsExtraResult(MobileParam mobileParams, int extracver, out int extrasver, out int ratio)
        {
            var extralist = NewsService.GetExtraAppViewList(mobileParams, extracver, out extrasver, out ratio);

            var result = new NewsExtraResult()
            {
                NewsExtraList = extralist.ToList(),
                Count = extralist.Count,
                ServerViersion = extrasver,
                Ratio = ratio
            };
            return result;
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public NewsContentResult GetNewsContentResult(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount)
        {
            var contentlist = NewsService.GetNewsContentViewList(mobileParams, stamp, action, categoryIds, startnum, num, out totalCount);

            var result = new NewsContentResult()
            {
                NewsContentList = contentlist.ToList(),
                Total = totalCount,
                Count = contentlist.Count
            };
            return result;
        }

        #region Helper

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
