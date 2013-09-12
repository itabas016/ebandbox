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
        public IDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IDbContextService>();
                }

                return _dbContextService;
            }
        } private IDbContextService _dbContextService;

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
                NormalURL="",
                HDURL="",
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

            var exsit = dbContextService.Exists<NewsImageInfo>(x=>x.Id == 1);
            Assert.Equal(true, exsit);

            var del_value = dbContextService.Delete<NewsImageInfo>(imagemodel);
            Console.WriteLine(del_value);

        }
    }
}
