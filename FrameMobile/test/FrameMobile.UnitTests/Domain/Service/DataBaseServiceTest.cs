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
        public IDataBaseService _dataBaseService { get; set; }

        public DataBaseServiceTest()
        {
            _dataBaseService = ObjectFactory.GetInstance<IDataBaseService>();
        }

        [Fact]
        public void AddTest()
        {
            var imagemodel = new NewsImageInfo()
            {
                Id = 1,
                NewsId = 1,
                Height = 100,
                Width = 200,
                URL = "test.com",
                Status = 1,
                CreateDateTime = DateTime.Now
            };

            _dataBaseService.Add<NewsImageInfo>(imagemodel);
        }
    }
}
