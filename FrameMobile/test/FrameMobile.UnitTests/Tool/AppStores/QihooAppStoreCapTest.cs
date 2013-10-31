using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QihooAppStoreCap.Invocation;
using Xunit;

namespace FrameMobile.UnitTests.Tool.AppStores
{
    public class QihooAppStoreCapTest
    {
        [Fact]
        public void RequestTest()
        {
            var app = new GetApps();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var data = app.GetData(parameters);
            Console.WriteLine(data);
        }

        [Fact]
        public void GenerateUrlTest()
        {

        }
    }
}
