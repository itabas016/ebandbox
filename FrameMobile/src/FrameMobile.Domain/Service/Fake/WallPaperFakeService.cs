using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Theme;

namespace FrameMobile.Domain.Service
{
    public class WallPaperFakeService : ThemeDbContextService, IWallPaperService
    {
        public IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams)
        {
            #region instance
            var cate1 = new WallPaperCategory()
                {
                    Id = 1,
                    Name = "风景",
                    CategoryLogoUrl = "",
                    OrderNumber = 1,
                    CreateDateTime = DateTime.Now,
                    Comment = "",
                    Status = 1
                };

            var cate2 = new WallPaperCategory()
            {
                Id = 2,
                Name = "卡通",
                CategoryLogoUrl = "",
                OrderNumber = 2,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };
            #endregion

            var categorylist = new List<WallPaperCategory>() { cate1, cate2 };

            var result = categorylist.To<IList<WallPaperCategoryView>>();

            return result;
        }

        public IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams)
        {
            #region instance
            var subcate1 = new WallPaperSubCategory()
            {
                Id = 1,
                Name = "大好河山",
                CategoryId = 1,
                SubCategoryLogoUrl = "",
                OrderNumber = 1,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };

            var subcate2 = new WallPaperSubCategory()
            {
                Id = 2,
                Name = "旖旎风光",
                CategoryId = 1,
                SubCategoryLogoUrl = "",
                OrderNumber = 3,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };

            var subcate3 = new WallPaperSubCategory()
            {
                Id = 3,
                Name = "日本动漫",
                CategoryId = 2,
                SubCategoryLogoUrl = "",
                OrderNumber = 2,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };
            #endregion

            var subcategorylist = new List<WallPaperSubCategory>() { subcate1, subcate2, subcate3 };

            var result = subcategorylist.To<IList<WallPaperSubCategoryView>>();

            return result;
        }

        public IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, string categoryIds, string subcategoryIds, int startnum, int num, out int totalCount)
        {
            var wallpaperlist = FakeWallPaperList();

            var result = wallpaperlist.To<IList<WallPaperView>>();

            totalCount = 4;

            return result;
        }

        public WallPaperView GetWallPaperViewDetail(MobileParam mobileParams, int wallPaperId)
        {
            var wallpaperlist = FakeWallPaperList();

            var wallpaper = wallpaperlist.Single<WallPaper>(x => x.Id == wallPaperId);

            var result = wallpaper.To<WallPaperView>();

            return result;
        }

        private IList<WallPaper> FakeWallPaperList()
        {
            #region instance
            var wall1 = new WallPaper()
            {
                Id = 1,
                Titile = "杭州西湖",
                CategoryId = 1,
                SubCategoryId = 1,
                Rating = 1,
                ThumbnailUrl = "",
                OriginalUrl = "",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 1000,
                OrderNumber = 12,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall2 = new WallPaper()
            {
                Id = 2,
                Titile = "安徽黄山",
                CategoryId = 1,
                SubCategoryId = 2,
                Rating = 1,
                ThumbnailUrl = "",
                OriginalUrl = "",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 100,
                OrderNumber = 15,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall3 = new WallPaper()
            {
                Id = 3,
                Titile = "进击的巨人",
                CategoryId = 2,
                SubCategoryId = 3,
                Rating = 3,
                ThumbnailUrl = "",
                OriginalUrl = "",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 100,
                OrderNumber = 20,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall4 = new WallPaper()
            {
                Id = 4,
                Titile = "哆啦A梦",
                CategoryId = 2,
                SubCategoryId = 3,
                Rating = 3,
                ThumbnailUrl = "",
                OriginalUrl = "",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 100,
                OrderNumber = 2,
                CreateDateTime = DateTime.Now,
                Status = 0
            };
            #endregion

            var wallpaperlist = new List<WallPaper>() { wall1, wall2, wall3, wall4 };

            return wallpaperlist;
        }
    }
}
