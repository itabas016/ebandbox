using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QihooAppStoreCap.Invocation;
using QihooAppStoreCap.Service;
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

        [Fact]
        public void DataConvertServiceTest()
        {
            DataConvertService service = new DataConvertService();
            var content = MockResponse();

            var result = service.DeserializeBase(content);

            Console.WriteLine(result.Total);

            if (result!= null)
            {
                Console.WriteLine(result.QihooApplist.Count);
            }
        }

        private string MockResponse()
        {
            var response = string.Empty;
            using (var sr = new StreamReader("Files\\QihoogetAppResponse.txt"))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }
    }
}
