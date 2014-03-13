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

            var wallpaperlist = WallPaperService.GetWallPaperListByScreenType(screenType);
            var wallpaperRelateMobileProperty = WallPaperService.GetWallPaperRelateMobilePropertyList(property.Id);

            if (categoryId == 0 && topicId == 0)
            {
                var wallpaperviewlist = from p in wallpaperlist
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
                totalCount = wallpaperviewlist.Count();
                return wallpaperviewlist.ToList();
            }

            if (categoryId == 0 && topicId != 0)
            {
                var wallpaperRelateTopiclist = WallPaperService.GetWallPaperRelateTopicList(topicId);

                var latestwallpaperlistbytopic = from p in wallpaperlist
                                                 join pt in wallpaperRelateTopiclist on p.Id equals pt.WallPaperId
                                                 join pm in wallpaperRelateMobileProperty on p.Id equals pm.WallPaperId
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
                var wallpaperRelateCategorylist = WallPaperService.GetWallPaperRelateCategoryList(categoryId);

                var latestwallpaperlistbycategory = from p in wallpaperlist
                                                    join pc in wallpaperRelateCategorylist on p.Id equals pc.WallPaperId
                                                    join pm in wallpaperRelateMobileProperty on p.Id equals pm.WallPaperId
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

            var wallpaperlist = WallPaperService.GetWallPaperListByScreenType(screenType);
            var wallpaperRelateMobileProperty = WallPaperService.GetWallPaperRelateMobilePropertyList(property.Id);

            if (categoryId == 0 && topicId == 0)
            {
                var wallpaperviewlist = from p in wallpaperlist
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
                totalCount = wallpaperviewlist.Count();
                return wallpaperviewlist.ToList();
            }

            if (categoryId == 0 && topicId != 0)
            {
                var wallpaperRelateTopiclist = WallPaperService.GetWallPaperRelateTopicList(topicId);

                var hottestwallpaperlistbytopic = from p in wallpaperlist
                                                  join pt in wallpaperRelateTopiclist on p.Id equals pt.WallPaperId
                                                  join pm in wallpaperRelateMobileProperty on p.Id equals pm.WallPaperId
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
                var wallpaperRelateCategorylist = WallPaperService.GetWallPaperRelateCategoryList(categoryId);

                var hottestwallpaperlistbycategory = from p in wallpaperlist
                                                     join pc in wallpaperRelateCategorylist on p.Id equals pc.WallPaperId
                                                     join pm in wallpaperRelateMobileProperty on p.Id equals pm.WallPaperId
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
