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
    public class DataBaseServiceTest : TestBase
    {
        public IDataBaseService dataBaseService
        {
            get
            {
                if (_databaseService == null)
                {
                    _databaseService = ObjectFactory.GetInstance<IDataBaseService>();
                }

                return _databaseService;
            }
        } private IDataBaseService _databaseService;

        public DataBaseServiceTest()
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

            var add_value = dataBaseService.Add<NewsImageInfo>(imagemodel);
            Console.WriteLine(add_value);

            imagemodel.Status = 0;
            
            var update_value = dataBaseService.Update<NewsImageInfo>(imagemodel);
            Assert.Equal(0, imagemodel.Status);
            Console.WriteLine(imagemodel.Status);

            var find_model = dataBaseService.Find<NewsImageInfo>(x => x.Id == 1);
            Assert.Equal(1, find_model.Count);
            Console.WriteLine(find_model.Count);

            var single_model = dataBaseService.Single<NewsImageInfo>(1);
            Assert.Equal(1, single_model.Id);

            var exsit = dataBaseService.Exists<NewsImageInfo>(x=>x.Id == 1);
            Assert.Equal(true, exsit);

            var del_value = dataBaseService.Delete<NewsImageInfo>(imagemodel);
            Console.WriteLine(del_value);

        }
    }
}
