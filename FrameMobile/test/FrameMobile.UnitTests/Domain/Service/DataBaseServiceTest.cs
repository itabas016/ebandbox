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
}
