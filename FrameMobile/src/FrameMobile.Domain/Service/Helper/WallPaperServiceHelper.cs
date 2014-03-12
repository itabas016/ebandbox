using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;
using FrameMobile.Model.Theme;
using StructureMap;

namespace FrameMobile.Domain.Service
{
    public class WallPaperServiceHelper : ThemeDbContextService, IWallPaperServiceHelper
    {
        private IWallPaperService _wallPaperService;
        public IWallPaperService WallPaperService
        {
            get
            {
                if (_wallPaperService == null)
                {
                    _wallPaperService = ObjectFactory.GetInstance<IWallPaperService>();
                }
                return _wallPaperService;
            }
            set
            {
                _wallPaperService = value;
            }
        }

        [ServiceCache(ClientType = RedisClientManagerType.ThemeCache)]
        public IList<WallPaperView> GetLatestWallPaperViewList(MobileParam mobileParams, MobileProperty property, int screenType, int categoryId, int topicId, int subcategoryId, out int totalCount)
        {
            totalCount = 0;
            if (categoryId == 0 && topicId != 0)
            {
                var latestwallpaperlistbytopic = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                 join pt in WallPaperService.GetWallPaperRelateTopicList(topicId) on p.Id equals pt.TopicId
                                                 join pm in WallPaperService.GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
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
            if (categoryId != 0)
            {
                var latestwallpaperlistbycategory = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                    join pc in WallPaperService.GetWallPaperRelateCategoryList(categoryId) on p.Id equals pc.WallPaperId
                                                    join pm in WallPaperService.GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
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

        [ServiceCache(ClientType = RedisClientManagerType.ThemeCache)]
        public IList<WallPaperView> GetHottestWallPaperViewList(MobileParam mobileParams, MobileProperty property, int screenType, int categoryId, int topicId, int subcategoryId, out int totalCount)
        {
            totalCount = 0;
            if (categoryId == 0 && topicId != 0)
            {
                var hottestwallpaperlistbytopic = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                  join pt in WallPaperService.GetWallPaperRelateTopicList(topicId) on p.Id equals pt.TopicId
                                                  join pm in WallPaperService.GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
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
            if (categoryId != 0)
            {
                var hottestwallpaperlistbycategory = from p in dbContextService.Find<WallPaper>(x => x.Status == 1 && x.ScreenType == screenType)
                                                     join pc in WallPaperService.GetWallPaperRelateCategoryList(categoryId) on p.Id equals pc.WallPaperId
                                                     join pm in WallPaperService.GetWallPaperRelateMobilePropertyList(property.Id) on p.Id equals pm.WallPaperId
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
    }
}
