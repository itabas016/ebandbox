using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using NCore;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class CommonServiceTest : TestBase
    {
        [Fact(Skip = "MySqlUpdate")]
        public void ReplaceNewsContent()
        {
            var startTime = new DateTime(2013, 10, 28, 0, 0, 0);
            var endTime = new DateTime(2013, 10, 28, 3, 0, 0);

            var service = new CommonService();
            service.UpdateNews(startTime, endTime);
        }
    }
}
