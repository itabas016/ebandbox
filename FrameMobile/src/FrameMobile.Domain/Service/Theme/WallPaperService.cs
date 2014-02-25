﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Theme;
using FrameMobile.Model.Mobile;
using FrameMobile.Common;

namespace FrameMobile.Domain.Service
{
    public class WallPaperService : ThemeDbContextService, IWallPaperService
    {
        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public MobileProperty GetMobileProperty(MobileParam mobileParams)
        {
            var brand = mobileParams.Manufacturer.ToLower();
            var resolution = mobileParams.Resolution.ToLower();

            var mobileproperty = from p in dbContextService.All<MobileProperty>()
                                 join b in dbContextService.All<MobileBrand>() on p.BrandId equals b.Id
                                 join r in dbContextService.All<MobileResolution>() on p.ResolutionId equals r.Id
                                 where b.Value == brand && r.Value == resolution
                                 select new MobileProperty
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     BrandId = p.BrandId,
                                     ResolutionId = p.ResolutionId,
                                     HardwareId = p.HardwareId,
                                     Status = p.Status
                                 };
            return mobileproperty.SingleOrDefault<MobileProperty>();
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<ThemeConfigView> GetConfigViewList(MobileParam mobileParams, int type)
        {
            var configlist = dbContextService.Find<ThemeConfig>(x => x.Status == 1 && x.Type == type);
            return configlist.To<IList<ThemeConfigView>>();
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new WallPaperCategory().ReturnThemeInstance<WallPaperCategory>(cver, out sver);

            categorylist.GetCompleteInstance<WallPaperCategory>();
            return categorylist.To<IList<WallPaperCategoryView>>();
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var subcategorylist = new WallPaperSubCategory().ReturnThemeInstance<WallPaperSubCategory>(cver, out sver);
            return subcategorylist.To<IList<WallPaperSubCategoryView>>();
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var topiclist = new WallPaperTopic().ReturnThemeInstance<WallPaperTopic>(cver, out sver);
            return topiclist.To<IList<WallPaperTopicView>>();
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int screenType, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount)
        {
            var property = GetMobileProperty(mobileParams);

            var result = new List<WallPaperView>();
            totalCount = 0;

            var field = sort == 0 ? Const.SORT_DOWNLOADNUMBER : Const.SORT_PUBLISHTIME;
            var sortParam = typeof(WallPaper).GetProperty(field);

            switch (sort)
            {
                case 1:
                    result = GetLatestWallPaperViewList(mobileParams, property, screenType, categoryId, topicId, subcategoryId, out totalCount);
                    break;
                default:
                    result = GetHottestWallPaperViewList(mobileParams, property, screenType, categoryId, topicId, subcategoryId, out totalCount);
                    break;
            }
            return result.Skip(startnum - 1).Take(num).ToList();
        }

        #region Heleper

        public List<WallPaperView> GetLatestWallPaperViewList(MobileParam mobileParams, MobileProperty property, int screenType, int categoryId, int topicId, int subcategoryId, out int totalCount)
        {
            totalCount = 0;
            if (categoryId == 0 && topicId != 0)
            {
                var latestwallpaperlistbytopic = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                 join pt in GetWallPaperRelateTopicList(topicId) on p.Id equals pt.TopicId
                                                 join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                 orderby p.PublishTime descending
                                                 select new WallPaperView
                                                 {
                                                     Id = p.Id,
                                                     Title = p.Title,
                                                     ThumbnailUrl = p.ThumbnailName.GetCompleteThumbnailUrl(mobileParams),
                                                     OriginalUrl = p.OriginalName.GetCompleteOriginalUrl(mobileParams),
                                                     DownloadNumber = p.DownloadNumber,
                                                     PublishTime = p.PublishTime
                                                 };
                totalCount = latestwallpaperlistbytopic.Count();
                return latestwallpaperlistbytopic.ToList();
            }
            if (categoryId == 0 && topicId == 0)
            {
                var latestwallpaperlistbycategory = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                    join pc in GetWallPaperRelateCategoryList(categoryId) on p.Id equals pc.WallPaperId
                                                    join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                    orderby p.PublishTime descending
                                                    select new WallPaperView
                                                    {
                                                        Id = p.Id,
                                                        Title = p.Title,
                                                        ThumbnailUrl = p.ThumbnailName.GetCompleteThumbnailUrl(mobileParams),
                                                        OriginalUrl = p.OriginalName.GetCompleteOriginalUrl(mobileParams),
                                                        DownloadNumber = p.DownloadNumber,
                                                        PublishTime = p.PublishTime
                                                    };
                totalCount = latestwallpaperlistbycategory.Count();
                return latestwallpaperlistbycategory.ToList();
            }
            return new List<WallPaperView>();
        }

        public List<WallPaperView> GetHottestWallPaperViewList(MobileParam mobileParams, MobileProperty property, int screenType, int categoryId, int topicId, int subcategoryId, out int totalCount)
        {
            totalCount = 0;
            if (categoryId == 0 && topicId != 0)
            {
                var hottestwallpaperlistbytopic = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                  join pt in GetWallPaperRelateTopicList(topicId) on p.Id equals pt.TopicId
                                                  join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                  orderby p.DownloadNumber descending
                                                  select new WallPaperView
                                                  {
                                                      Id = p.Id,
                                                      Title = p.Title,
                                                      ThumbnailUrl = p.ThumbnailName.GetCompleteThumbnailUrl(mobileParams),
                                                      OriginalUrl = p.OriginalName.GetCompleteOriginalUrl(mobileParams),
                                                      DownloadNumber = p.DownloadNumber,
                                                      PublishTime = p.PublishTime
                                                  };
                totalCount = hottestwallpaperlistbytopic.Count();
                return hottestwallpaperlistbytopic.ToList();
            }
            if (categoryId == 0 && topicId == 0)
            {
                var hottestwallpaperlistbycategory = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                     join pc in GetWallPaperRelateCategoryList(categoryId) on p.Id equals pc.WallPaperId
                                                     join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                     orderby p.DownloadNumber descending
                                                     select new WallPaperView
                                                     {
                                                         Id = p.Id,
                                                         Title = p.Title,
                                                         ThumbnailUrl = p.ThumbnailName.GetCompleteThumbnailUrl(mobileParams),
                                                         OriginalUrl = p.OriginalName.GetCompleteOriginalUrl(mobileParams),
                                                         DownloadNumber = p.DownloadNumber,
                                                         PublishTime = p.PublishTime
                                                     };
                totalCount = hottestwallpaperlistbycategory.Count();
                return hottestwallpaperlistbycategory.ToList();
            }
            return new List<WallPaperView>();
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperRelateCategory> GetWallPaperRelateCategoryList(int categoryId)
        {
            if (categoryId == 0)
            {
                return dbContextService.Find<WallPaperRelateCategory>(x => x.Status == 1).ToList();
            }
            var categorywallpaperlist = dbContextService.Find<WallPaperRelateCategory>(x => x.CategoryId == categoryId && x.Status == 1).ToList();
            return categorywallpaperlist;
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperRelateSubCategory> GetWallPaperRelateSubCategoryList(int subcategoryId)
        {
            if (subcategoryId == 0)
            {
                return dbContextService.Find<WallPaperRelateSubCategory>(x => x.Status == 1).ToList();
            }
            var subcategorywallpaperlist = dbContextService.Find<WallPaperRelateSubCategory>(x => x.SubCategoryId == subcategoryId && x.Status == 1).ToList();
            return subcategorywallpaperlist;
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperRelateTopic> GetWallPaperRelateTopicList(int topicId)
        {
            var topicwallpaperlist = dbContextService.Find<WallPaperRelateTopic>(x => x.TopicId == topicId && x.Status == 1).ToList();
            return topicwallpaperlist;
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public IList<WallPaperRelateMobileProperty> GetWallPaperRelateMobilePropertyList(int propertyId)
        {
            var mobilePropertylist = dbContextService.Find<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == propertyId && x.Status == 1).ToList();
            return mobilePropertylist;
        }

        [ServiceCache(ClientType=RedisClientManagerType.ThemeCache)]
        public WallPaperView GetWallPaperViewDetail(MobileParam mobileParams, int wallPaperId)
        {
            var property = GetMobileProperty(mobileParams);

            var wallpaper = from p in dbContextService.Find<WallPaper>(x => x.Id == wallPaperId)
                            join pm in dbContextService.Find<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == property.Id) on p.Id equals pm.WallPaperId
                            select new WallPaper
                            {
                                Id = p.Id,
                                Title = p.Title,
                                ThumbnailName = p.ThumbnailName,
                                OriginalName = p.OriginalName,
                                DownloadNumber = p.DownloadNumber,
                                Rating = p.Status,
                                PublishTime = p.PublishTime
                            };

            return wallpaper.To<WallPaperView>();
        }

        #endregion
    }
}
