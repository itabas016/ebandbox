using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model.News;
using Moq;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class DataBaseServiceTest : DataBaseServiceTestBase
    {
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
            dataBaseService.Add<NewsImageInfo>(imagemodel);
        }
    }

    public class DataBaseServiceTestBase : TestBase
    {

        Mock<IDataBaseService> _dataBaseServiceMock;
        IDataBaseService DataBaseService;
        public DataBaseService dataBaseService;

        public DataBaseServiceTestBase()
        {
            _dataBaseServiceMock = new Mock<IDataBaseService>();
            DataBaseService = _dataBaseServiceMock.Object;
            dataBaseService = new DataBaseService();
        }
    }
}
