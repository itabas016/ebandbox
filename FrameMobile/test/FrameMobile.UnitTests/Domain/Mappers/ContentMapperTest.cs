using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.News;
using Xunit;
using FrameMobile.Domain;
using FrameMobile.Model.ThirdPart;

namespace FrameMobile.UnitTests.Domain.Mappers
{
    public class ContentMapperTest : TestBase
    {
        [Fact]
        public void TouTiaoContentTest()
        {
            var m = new TouTiaoContent();

            m.DiggCount = 1;
            m.NewsId = 2730766760;

            m.PublishTime = 1378033125;

            var x = m.To<NewsContent>();

            Assert.Equal(2730766760, x.NewsId);
            Console.WriteLine(x.PublishTime);
        }
    }
}
