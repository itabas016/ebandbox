using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using FrameMobile.Model.News;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Extension
{
    public class NewsExtensionTest : TestBase
    {
        [Fact]
        public void CheckVersionTest()
        {
            var ret = NewsExtensions.CheckVersion(new NewsSource());
            Console.WriteLine(ret);
        }
    }
}
