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
                    Name = "美女",
                    CategoryLogoUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th1123.jpg",
                    OrderNumber = 1,
                    CreateDateTime = DateTime.Now,
                    Comment = "",
                    Status = 1
                };

            var cate2 = new WallPaperCategory()
            {
                Id = 2,
                Name = "动漫",
                CategoryLogoUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th1111.jpg",
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
                Name = "美女之家",
                CategoryId = 1,
                SubCategoryLogoUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th1123.jpg",
                OrderNumber = 1,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };

            var subcate2 = new WallPaperSubCategory()
            {
                Id = 2,
                Name = "模特",
                CategoryId = 1,
                SubCategoryLogoUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th1124.jpg",
                OrderNumber = 3,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };

            var subcate3 = new WallPaperSubCategory()
            {
                Id = 3,
                Name = "动漫1",
                CategoryId = 2,
                SubCategoryLogoUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th1112.jpg",
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
                Titile = "美女01",
                CategoryId = 1,
                SubCategoryId = 1,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th04.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th04.jpg",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 1000,
                OrderNumber = 12,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall01 = new WallPaper()
            {
                Id = 1,
                Titile = "美女01",
                CategoryId = 1,
                SubCategoryId = 1,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th05.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th05.jpg",
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
                Titile = "模特01",
                CategoryId = 1,
                SubCategoryId = 2,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th06.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th06.jpg",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 100,
                OrderNumber = 15,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall02 = new WallPaper()
            {
                Id = 2,
                Titile = "模特02",
                CategoryId = 1,
                SubCategoryId = 2,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th07.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th07.jpg",
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
                Titile = "动漫01",
                CategoryId = 2,
                SubCategoryId = 3,
                Rating = 3,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th01.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th01.jpg",
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
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th02.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th02.jpg",
                PublishTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                DownloadNumber = 100,
                OrderNumber = 2,
                CreateDateTime = DateTime.Now,
                Status = 0
            };
            #endregion

            var wallpaperlist = new List<WallPaper>() { wall1, wall01, wall02, wall2, wall3, wall4 };

            return wallpaperlist;
        }
    }
}
