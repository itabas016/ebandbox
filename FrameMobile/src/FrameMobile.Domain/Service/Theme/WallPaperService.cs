using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Theme;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public class WallPaperService : ThemeDbContextService, IWallPaperService
    {
        [ServiceCache]
        public MobileProperty GetMobileProperty(MobileParam mobileParams)
        {
            var brand = mobileParams.Manufacturer.ToLower();
            var resolution = mobileParams.Resolution.ToLower();

            var mobileproperty = from p in dbContextService.All<MobileProperty>()
                                 join b in dbContextService.All<MobileBrand>() on p.BrandId equals b.Id
                                 join r in dbContextService.All<MobileResolution>() on p.ResoulutionId equals r.Id
                                 where b.Value == brand && r.Value == resolution
                                 select new MobileProperty
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     BrandId = p.BrandId,
                                     ResoulutionId = p.ResoulutionId,
                                     HardwareId = p.HardwareId,
                                     Status = p.Status
                                 };
            return mobileproperty.SingleOrDefault<MobileProperty>();
        }

        [ServiceCache]
        public IList<ThemeConfigView> GetConfigViewList(MobileParam mobileParams, int type)
        {
            var configlist = dbContextService.Find<ThemeConfig>(x => x.Status == 1 && x.Type == type);
            return configlist.To<IList<ThemeConfigView>>();
        }

        [ServiceCache]
        public IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new WallPaperCategory().ReturnThemeInstance<WallPaperCategory>(cver, out sver);
            return categorylist.To<IList<WallPaperCategoryView>>();
        }

        [ServiceCache]
        public IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var subcategorylist = new WallPaperSubCategory().ReturnThemeInstance<WallPaperSubCategory>(cver, out sver);
            return subcategorylist.To<IList<WallPaperSubCategoryView>>();
        }

        [ServiceCache]
        public IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var topiclist = new WallPaperTopic().ReturnThemeInstance<WallPaperTopic>(cver, out sver);
            return topiclist.To<IList<WallPaperTopicView>>();
        }

        [ServiceCache]
        public IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount)
        {
            var property = GetMobileProperty(mobileParams);

            var result = new List<WallPaperView>();
            totalCount = 0;

            var field = sort == 0 ? "DownloadNumber" : "PublishTime";
            var sortParam = typeof(WallPaper).GetProperty(field);

            switch (sort)
            {
                case 1:
                    result = GetLatestWallPaperViewList(property, categoryId, topicId, subcategoryId, out totalCount);
                    break;
                default:
                    result = GetHottestWallPaperViewList(property, categoryId, topicId, subcategoryId, out totalCount);
                    break;
            }
            return result;
        }

        #region Heleper

        public List<WallPaperView> GetLatestWallPaperViewList(MobileProperty property, int categoryId, int topicId, int subcategoryId, out int totalCount)
        {
            totalCount = 0;
            if (categoryId == 0 && topicId != 0)
            {
                var latestwallpaperlistbytopic = from p in dbContextService.Find<WallPaper>(x => x.Status == 1)
                                                 join pt in GetWallPaperRelateTopicList(topicId) on p.Id equals pt.TopicId
                                                 join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                 orderby p.PublishTime descending
                                                 select new WallPaper
                                                 {
                                                     Id = p.Id,
                                                     Titile = p.Titile,
                                                     ThumbnailName = p.ThumbnailName,
                                                     OriginalName = p.OriginalName,
                                                     DownloadNumber = p.DownloadNumber,
                                                     Rating = p.Status,
                                                     PublishTime = p.PublishTime
                                                 };
                totalCount = latestwallpaperlistbytopic.Count();
                return latestwallpaperlistbytopic.To<IList<WallPaperView>>().ToList();
            }
            if (categoryId == 0 && topicId == 0)
            {
                var latestwallpaperlistbycategory = from p in dbContextService.Find<WallPaper>(x => x.Status == 1)
                                                    join pc in GetWallPaperRelateCategoryList(categoryId) on p.Id equals pc.WallPaperId
                                                    join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                    orderby p.PublishTime descending
                                                    select new WallPaper
                                                    {
                                                        Id = p.Id,
                                                        Titile = p.Titile,
                                                        ThumbnailName = p.ThumbnailName,
                                                        OriginalName = p.OriginalName,
                                                        DownloadNumber = p.DownloadNumber,
                                                        Rating = p.Status,
                                                        PublishTime = p.PublishTime
                                                    };
                totalCount = latestwallpaperlistbycategory.Count();
                return latestwallpaperlistbycategory.To<IList<WallPaperView>>().ToList();
            }
            return new List<WallPaperView>();
        }

        public List<WallPaperView> GetHottestWallPaperViewList(MobileProperty property, int categoryId, int topicId, int subcategoryId, out int totalCount)
        {
            totalCount = 0;
            if (categoryId == 0 && topicId != 0)
            {
                var hottestwallpaperlistbytopic = from p in dbContextService.Find<WallPaper>(x => x.Status == 1)
                                                 join pt in GetWallPaperRelateTopicList(topicId) on p.Id equals pt.TopicId
                                                 join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                 orderby p.DownloadNumber descending
                                                 select new WallPaper
                                                 {
                                                     Id = p.Id,
                                                     Titile = p.Titile,
                                                     ThumbnailName = p.ThumbnailName,
                                                     OriginalName = p.OriginalName,
                                                     DownloadNumber = p.DownloadNumber,
                                                     Rating = p.Status,
                                                     PublishTime = p.PublishTime
                                                 };
                totalCount = hottestwallpaperlistbytopic.Count();
                return hottestwallpaperlistbytopic.To<IList<WallPaperView>>().ToList();
            }
            if (categoryId == 0 && topicId == 0)
            {
                var hottestwallpaperlistbycategory = from p in dbContextService.Find<WallPaper>(x => x.Status == 1)
                                                    join pc in GetWallPaperRelateCategoryList(categoryId) on p.Id equals pc.WallPaperId
                                                    join pm in GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
                                                    orderby p.DownloadNumber descending
                                                    select new WallPaper
                                                    {
                                                        Id = p.Id,
                                                        Titile = p.Titile,
                                                        ThumbnailName = p.ThumbnailName,
                                                        OriginalName = p.OriginalName,
                                                        DownloadNumber = p.DownloadNumber,
                                                        Rating = p.Status,
                                                        PublishTime = p.PublishTime
                                                    };
                totalCount = hottestwallpaperlistbycategory.Count();
                return hottestwallpaperlistbycategory.To<IList<WallPaperView>>().ToList();
            }
            return new List<WallPaperView>();
        }

        [ServiceCache]
        public IList<WallPaperRelateCategory> GetWallPaperRelateCategoryList(int categoryId)
        {
            if (categoryId == 0)
            {
                return dbContextService.Find<WallPaperRelateCategory>(x => x.Status == 1).ToList();
            }
            var categorywallpaperlist = dbContextService.Find<WallPaperRelateCategory>(x => x.CategoryId == categoryId && x.Status == 1).ToList();
            return categorywallpaperlist;
        }

        [ServiceCache]
        public IList<WallPaperRelateSubCategory> GetWallPaperRelateSubCategoryList(int subcategoryId)
        {
            if (subcategoryId == 0)
            {
                return dbContextService.Find<WallPaperRelateSubCategory>(x => x.Status == 1).ToList();
            }
            var subcategorywallpaperlist = dbContextService.Find<WallPaperRelateSubCategory>(x => x.SubCategoryId == subcategoryId && x.Status == 1).ToList();
            return subcategorywallpaperlist;
        }

        [ServiceCache]
        public IList<WallPaperRelateTopic> GetWallPaperRelateTopicList(int topicId)
        {
            var topicwallpaperlist = dbContextService.Find<WallPaperRelateTopic>(x => x.TopicId == topicId && x.Status == 1).ToList();
            return topicwallpaperlist;
        }

        [ServiceCache]
        public IList<WallPaperRelateMobileProperty> GetWallPaperRelateMobilePropertyList(int propertyId)
        {
            var mobilePropertylist = dbContextService.Find<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == propertyId && x.Status == 1).ToList();
            return mobilePropertylist;
        }

        [ServiceCache]
        public WallPaperView GetWallPaperViewDetail(MobileParam mobileParams, int wallPaperId)
        {
            var property = GetMobileProperty(mobileParams);

            var wallpaper = from p in dbContextService.Find<WallPaper>(x => x.Id == wallPaperId)
                            join pm in dbContextService.Find<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == property.Id) on p.Id equals pm.WallPaperId
                            select new WallPaper
                            {
                                Id = p.Id,
                                Titile = p.Titile,
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
