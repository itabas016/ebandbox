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
using FrameMobile.Model.Radar;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public class NewsService : NewsDbContextService, INewsService
    {
        public INewsServiceHelper NewsServiceHelper
        {
            get
            {
                if (_newsServiceHelper == null)
                    _newsServiceHelper = ObjectFactory.GetInstance<INewsServiceHelper>();

                return _newsServiceHelper;
            }
            set
            {
                _newsServiceHelper = value;
            }
        }
        private INewsServiceHelper _newsServiceHelper;

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsConfigView> GetConfigViewList(MobileParam mobileParams)
        {
            var configlist = dbContextService.Find<NewsConfig>(x => x.Status == 1);
            return configlist.To<IList<NewsConfigView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsSourceView> GetSourceViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var sourcelist = new NewsSource().ReturnNewsInstance<NewsSource>(cver, out sver);
            return sourcelist.To<IList<NewsSourceView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsExtraAppView> GetExtraAppViewList(MobileParam mobileParams, int cver, out int sver, out int ratio)
        {
            ratio = NewsServiceHelper.GetExtraRatioByChannel(mobileParams);
            var extraAppList = new NewsExtraApp().ReturnNewsInstance<NewsExtraApp>(cver, out sver);
            return extraAppList.To<IList<NewsExtraAppView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<OlderNewsExtraAppView> GetOlderExtraAppViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var ratio = 0;
            var extraappviewlist = GetExtraAppViewList(mobileParams, cver, out sver, out ratio);
            return extraappviewlist.To<IList<OlderNewsExtraAppView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsInfAddressView> GetInfAddressViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var infAddressList = new NewsInfAddress().ReturnNewsInstance<NewsInfAddress>(cver, out sver);
            return infAddressList.To<IList<NewsInfAddressView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new NewsCategory().ReturnNewsInstance<NewsCategory>(cver, out sver);
            return categorylist.To<IList<NewsCategoryView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams)
        {
            var subcategorylist = dbContextService.Find<NewsSubCategory>(x => x.Status == 1);
            return subcategorylist.To<IList<NewsSubCategoryView>>();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsRadarView> GetNewsRadarViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var imageType = NewsServiceHelper.GetImageURLTypeByResolution(mobileParams);

            var radarlist = new RadarCategory().ReturnRadarInstance<RadarCategory>(cver, out sver);
            var subradarlist = new RadarElement().ReturnRadarInstance<RadarElement>(cver, out sver);

            return ConvertByRadar(imageType, radarlist, subradarlist);
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public IList<NewsContentView> GetNewsContentViewList(MobileParam mobileParams, long stamp, bool action, string categoryIds, int startnum, int num, out int totalCount)
        {
            var contentlist = new List<NewsContentView>();

            totalCount = 0;
            var categoryIdList = categoryIds.Split(';', '；').ToList().ToInt32List();

            contentlist = this.GetContentViewList(mobileParams, categoryIdList, stamp, action).ToList();

            totalCount = contentlist.Count;

            return contentlist.Skip(startnum - 1).Take(num).ToList();
        }

        [ServiceCache(ClientType = RedisClientManagerType.NewsCache)]
        public NewsCollectionView GetNewsCollectionView(MobileParam mobileParams, long stamp, int extracver, bool action, string categoryIds, int startnum, int num, out int extrasver, out int ratio, out int totalCount)
        {
            var collection = new NewsCollectionView();

            collection.NewsExtraResult = NewsServiceHelper.GetNewsExtraResult(mobileParams, extracver, out extrasver, out ratio);
            collection.NewsContentResult = NewsServiceHelper.GetNewsContentResult(mobileParams, stamp, action, categoryIds, startnum, num, out totalCount);
            return collection;
        }

        #region Helper

        public IList<NewsContentView> GetContentViewList(MobileParam mobileParams, List<int> categoryIds, long stamp, bool action)
        {
            var contentViewList = new List<NewsContentView>();

            var extraAppList = NewsServiceHelper.GetNewsExtraAppList().ToList();
            var imageType = NewsServiceHelper.GetImageURLTypeByResolution(mobileParams);

            var stampTime = stamp.UTCStamp();
            var endDateTime = stampTime.AddDays(-3);

            switch (action)
            {
                case false:
                    contentViewList = NewsServiceHelper.GetOldestNewsContentView(categoryIds, extraAppList, imageType, endDateTime, stampTime).ToList();
                    break;
                default:
                    contentViewList = NewsServiceHelper.GetLatestNewsContentView(categoryIds, extraAppList, imageType, stampTime).ToList();
                    break;
            }

            var localContentList = NewsServiceHelper.GetLocalContentViewList(extraAppList, imageType);

            contentViewList = InsertRange(contentViewList, localContentList);

            return contentViewList;
        }

        private IList<NewsRadarView> ConvertByRadar(int imageType, IList<RadarCategory> radarlist, IList<RadarElement> subradarlist)
        {
            var radarviewlist = radarlist.To<IList<NewsRadarView>>();
            var subradarviewlist = subradarlist.To<IList<NewsRadarElementView>>();

            foreach (var item in radarviewlist)
            {
                var radar = radarlist.Where(x => x.Id == item.Id).SingleOrDefault();
                item.LogoUrl = imageType == 1 ? radar.NormalLogoUrl : radar.HDLogoUrl;
                item.NewsRadarElementList = subradarviewlist.Where(x => x.RadarCategoryIds.Contains(item.Id)).ToList();
            }
            return radarviewlist;
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

        #endregion
    }
}
