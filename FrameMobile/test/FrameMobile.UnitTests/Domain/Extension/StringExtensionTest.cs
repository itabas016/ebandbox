using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Extension
{
    public class StringExtensionTest : TestBase
    {
        [Fact]
        public void UnixStampTest()
        {
            var ret = StringExtensions.UnixStamp(DateTime.Now);
            Console.WriteLine(ret);
        }

        [Fact]
        public void UTCStampTest()
        {
            var ret = StringExtensions.UTCStamp(1380002648);
            Console.WriteLine(ret);
        }
    }
}
