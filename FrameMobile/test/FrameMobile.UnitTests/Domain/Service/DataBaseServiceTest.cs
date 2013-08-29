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
    public class DataBaseServiceTest
    {
        Mock<IDataBaseService> _dataBaseServiceMock;
        IDataBaseService DataBaseService;

        DataBaseService dataBasesvr;

        public DataBaseServiceTest()
        {
            _dataBaseServiceMock = new Mock<IDataBaseService>();
            DataBaseService = _dataBaseServiceMock.Object;

            this.dataBasesvr = new DataBaseService();
            EntityMapping.ResetMapper();
            EntityMapping.Config();
            Bootstrapper.ConfigueInjection();
        }

        public void AddTest()
        {
            var imagemodel = new NewsImageInfo()
            {
                Id=1,
                NewsId=1,
                Height=100,
                Width=200,
                URL="test.com",
                Status=1,
                CreateDateTime=DateTime.Now
            };
            dataBasesvr.Add<NewsImageInfo>(imagemodel);

        }
    }
}
