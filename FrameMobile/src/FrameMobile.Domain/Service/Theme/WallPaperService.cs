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

        public IList<ThemeConfigView> GetConfigViewList(MobileParam mobileParams, int type)
        {
            var configlist = dbContextService.Find<ThemeConfig>(x => x.Status == 1 && x.Type == type);
            return configlist.To<IList<ThemeConfigView>>();
        }

        public IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var categorylist = new WallPaperCategory().ReturnThemeInstance<WallPaperCategory>(cver, out sver);
            return categorylist.To<IList<WallPaperCategoryView>>();
        }

        public IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var subcategorylist = new WallPaperSubCategory().ReturnThemeInstance<WallPaperSubCategory>(cver, out sver);
            return subcategorylist.To<IList<WallPaperSubCategoryView>>();
        }

        public IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams, int cver, out int sver)
        {
            var topiclist = new WallPaperTopic().ReturnThemeInstance<WallPaperTopic>(cver, out sver);
            return topiclist.To<IList<WallPaperTopicView>>();
        }

        public IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount)
        {
            var property = GetMobileProperty(mobileParams);

            var wallpaperlist = from p in dbContextService.Find<WallPaper>(x => x.Status == 1)
                                join pc in dbContextService.Find<WallPaperRelateCategory>(x => x.CategoryId == categoryId) on p.Id equals pc.WallPaperId
                                join pt in dbContextService.Find<WallPaperRelateTopic>(x => x.TopicId == topicId) on p.Id equals pt.TopicId
                                join pm in dbContextService.Find<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == property.Id) on p.Id equals pm.WallPaperId
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
            totalCount = wallpaperlist.Count();

            return wallpaperlist.To<IList<WallPaperView>>();
        }

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

    }
}
