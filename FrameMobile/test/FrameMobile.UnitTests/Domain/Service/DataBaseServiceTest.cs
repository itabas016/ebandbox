using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model.News;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class DataBaseServiceTest
    {
        public IDataBaseService DataBaseService { get; set; }

        public DataBaseServiceTest()
        {
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
            DataBaseService.Add<NewsImageInfo>(imagemodel);
        }
    }
}
