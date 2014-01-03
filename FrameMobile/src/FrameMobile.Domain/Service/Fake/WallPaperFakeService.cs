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
        public IList<ThemeConfigView> GetConfigViewList(MobileParam mobileParams, int type)
        {
            #region instance
            var config = new ThemeConfig()
            {
                Id = 1,
                Name = "壁纸分类",
                NameLowCase = "themecategory",
                Type = 1,
                Version = 1,
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var config2 = new ThemeConfig()
            {
                Id = 2,
                Name = "壁纸子分类",
                NameLowCase = "themesubcategory",
                Type = 1,
                Version = 2,
                Status = 0,
                CreateDateTime = DateTime.Now
            };

            var config3 = new ThemeConfig()
            {
                Id = 3,
                Name = "壁纸专题",
                NameLowCase = "themetopic",
                Version = 1,
                Type = 1,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            #endregion

            var configlist = new List<ThemeConfig>() { config, config2, config3 };

            var result = configlist.To<IList<ThemeConfigView>>();

            return result;
        }

        public IList<WallPaperCategoryView> GetCategoryViewList(MobileParam mobileParams, int cver, out int sver)
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

            sver = 1;
            return result;
        }

        public IList<WallPaperSubCategoryView> GetSubCategoryViewList(MobileParam mobileParams, int cver, out int sver)
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

            sver = 1;
            return result;
        }

        public IList<WallPaperTopicView> GetTopicViewList(MobileParam mobileParams, int cver, out int sver)
        {
            #region instance
            var topic1 = new WallPaperTopic()
            {
                Id = 1,
                Name = "新年快乐",
                TopicLogoUrl = "http://theme.kk874.com/ThemeResources/Logo/p1.jpg",
                OrderNumber = 1,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };

            var topic2 = new WallPaperTopic()
            {
                Id = 2,
                Name = "元旦快乐",
                TopicLogoUrl = "http://theme.kk874.com/ThemeResources/Logo/p2.jpg",
                OrderNumber = 2,
                CreateDateTime = DateTime.Now,
                Comment = "",
                Status = 1
            };
            #endregion

            var topiclist = new List<WallPaperTopic>() { topic1, topic2, topic2, topic1 };

            var result = topiclist.To<IList<WallPaperTopicView>>();

            sver = 1;
            return result;
        }

        public IList<WallPaperView> GetWallPaperViewList(MobileParam mobileParams, int categoryId, int topicId, int subcategoryId, int sort, int startnum, int num, out int totalCount)
        {
            var result = new List<WallPaperView>();

            if (sort == 0)
            {
                var hotwallpaperlist = FakeWallPaperList().Where(x => x.CategoryId == categoryId).OrderByDescending(x => x.DownloadNumber);
                result = hotwallpaperlist.To<IList<WallPaperView>>().ToList();
            }
            else if (sort == 1)
            {
                var latestwallpaperlist = FakeWallPaperList().Where(x => x.CategoryId == categoryId).OrderByDescending(x => x.PublishTime);
                result = latestwallpaperlist.To<IList<WallPaperView>>().ToList();
            }

            totalCount = result.Count;
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
                PublishTime = DateTime.Now.AddHours(-1),
                ModifiedTime = DateTime.Now,
                DownloadNumber = 23,
                OrderNumber = 12,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall01 = new WallPaper()
            {
                Id = 2,
                Titile = "美女01",
                CategoryId = 1,
                SubCategoryId = 1,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th05.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th05.jpg",
                PublishTime = DateTime.Now.AddHours(-5),
                ModifiedTime = DateTime.Now,
                DownloadNumber = 56,
                OrderNumber = 12,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall2 = new WallPaper()
            {
                Id = 3,
                Titile = "模特01",
                CategoryId = 1,
                SubCategoryId = 2,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th06.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th06.jpg",
                PublishTime = DateTime.Now.AddDays(-1),
                ModifiedTime = DateTime.Now,
                DownloadNumber = 10,
                OrderNumber = 15,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall02 = new WallPaper()
            {
                Id = 4,
                Titile = "模特02",
                CategoryId = 1,
                SubCategoryId = 2,
                Rating = 1,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th07.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th07.jpg",
                PublishTime = DateTime.Now.AddHours(-17),
                ModifiedTime = DateTime.Now,
                DownloadNumber = 40,
                OrderNumber = 15,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall3 = new WallPaper()
            {
                Id = 5,
                Titile = "动漫01",
                CategoryId = 2,
                SubCategoryId = 3,
                Rating = 3,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th01.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th01.jpg",
                PublishTime = DateTime.Now.AddMinutes(-2),
                ModifiedTime = DateTime.Now,
                DownloadNumber = 5,
                OrderNumber = 20,
                CreateDateTime = DateTime.Now,
                Status = 1
            };

            var wall4 = new WallPaper()
            {
                Id = 6,
                Titile = "哆啦A梦",
                CategoryId = 2,
                SubCategoryId = 3,
                Rating = 3,
                ThumbnailUrl = "http://theme.kk874.com/ThemeResources/Thumbnail/th02.jpg",
                OriginalUrl = "http://theme.kk874.com/ThemeResources/Original/th02.jpg",
                PublishTime = DateTime.Now.AddMinutes(-13),
                ModifiedTime = DateTime.Now,
                DownloadNumber = 6,
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
