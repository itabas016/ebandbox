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
        public void test()
        {
            var m = new TouTiaoContent();

            m.DiggCount = 1;
            m.Id = 3;

            var x = m.To<TouTiaoModel>();

            Console.WriteLine(x.NewsId);
        }
    }
}
