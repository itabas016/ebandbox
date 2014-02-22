using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model.News;
using Moq;
using StructureMap;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class DbContextServiceTest : TestBase
    {
        public INewsDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                }

                return _dbContextService;
            }
        } private INewsDbContextService _dbContextService;

        public DbContextServiceTest()
        {
        }

        [Fact]
        public void AllMethodTest()
        {
            var imagemodel = new NewsImageInfo()
            {
                Id = 1,
                NewsId = 1,
                NormalURL = "",
                HDURL = "",
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            var add_value = dbContextService.Add<NewsImageInfo>(imagemodel);
            Console.WriteLine(add_value);

            imagemodel.Status = 0;

            var update_value = dbContextService.Update<NewsImageInfo>(imagemodel);
            Assert.Equal(0, imagemodel.Status);
            Console.WriteLine(imagemodel.Status);

            var find_model = dbContextService.Find<NewsImageInfo>(x => x.Id == 1);
            Assert.Equal(1, find_model.Count);
            Console.WriteLine(find_model.Count);

            var single_model = dbContextService.Single<NewsImageInfo>(1);
            Assert.Equal(1, single_model.Id);

            var exsit = dbContextService.Exists<NewsImageInfo>(x => x.Id == 1);
            Assert.Equal(true, exsit);

            var del_value = dbContextService.Delete<NewsImageInfo>(imagemodel);
            Console.WriteLine(del_value);

        }

        [Fact]
        public void TouTiaoContentUpdate()
        {
            var contentList = dbContextService.Find<NewsContent>(x => x.SubCategoryId == 0);
            foreach (var item in contentList)
            {
                #region Case SubCategory
                switch (item.CategoryId)
                {
                    case 1:
                        item.SubCategoryId = 1;
                        item.CategoryId = 1;
                        break;
                    case 2:
                        item.SubCategoryId = 2;
                        item.CategoryId = 5;
                        break;
                    case 3:
                        item.SubCategoryId = 3;
                        item.CategoryId = 3;
                        break;
                    case 4:
                        item.SubCategoryId = 4;
                        item.CategoryId = 2;
                        break;
                    case 5:
                        item.SubCategoryId = 5;
                        item.CategoryId = 6;
                        break;
                    case 6:
                        item.SubCategoryId = 6;
                        item.CategoryId = 6;
                        break;
                    case 7:
                        item.SubCategoryId = 7;
                        item.CategoryId = 6;
                        break;
                    case 8:
                        item.SubCategoryId = 8;
                        item.CategoryId = 6;
                        break;
                    case 9:
                        item.SubCategoryId = 9;
                        item.CategoryId = 6;
                        break;

                    default:
                        item.SubCategoryId = 0;
                        item.CategoryId = 0;
                        break;
                }
                #endregion

                dbContextService.Update<NewsContent>(item);
            }
        }

        [Fact]
        public void IntinaizedNewsConfigTable()
        {
            var config = new NewsConfig()
            {
                Id = 1,
                Name = "newssource",
                Version = 0,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            var config2 = new NewsConfig()
            {
                Id = 2,
                Name = "newscategory",
                Version = 0,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            var config3 = new NewsConfig()
            {
                Id = 3,
                Name = "newsextraapp",
                Version = 0,
                Status = 1,
                CreateDateTime = DateTime.Now
            };
            var configlist = new List<NewsConfig>() { config, config2, config3 };

            dbContextService.Add<NewsConfig>(configlist);
        }

        [Fact]
        public void BatchInsertNewsContentTable()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            var example = dbContextService.Single<NewsContent>(x => x.Id == 44);

            for (int i = 10012; i <= 100000; i++)
            {
                var content = new NewsContent();
                content.NewsId = i;
                content.CategoryId = random.Next(0, 7);
                content.SubCategoryId = random.Next(0,10);
                content.ExtraAppId= random.Next(0,5);
                content.Title = example.Title;
                content.Summary = example.Summary;
                content.Site = example.Site;
                content.WAPURL = example.WAPURL;
                content.HDURL = example.HDURL;
                content.NormalURL = example.NormalURL;
                content.AppOpenURL = example.AppOpenURL;
                content.PublishTime = DateTime.Now.AddMinutes(random.Next(0, 5000));

                var outputId = dbContextService.Add<NewsContent>(content);

                Console.WriteLine((int)outputId);
            }
        }

    }
}
