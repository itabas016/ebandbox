using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QihooAppStoreCap;
using QihooAppStoreCap.Invocation;
using QihooAppStoreCap.Model;
using QihooAppStoreCap.Service;
using Xunit;

namespace FrameMobile.UnitTests.Tool.AppStores
{
    public class QihooAppStoreCapTest
    {
        private DataConvertService _service;
        private AppItemCap _cap;
        public QihooAppStoreCapTest()
        {
            _service = new DataConvertService();
            _cap = new AppItemCap();
        }

        [Fact]
        public void RequestTest()
        {
            var app = new GetApps();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var data = app.GetData(parameters);
            Console.WriteLine(data);
        }

        [Fact]
        public void CategoryTest()
        {
            var ret = _cap.GetAllCategory();
            ret.ForEach(x=>Console.WriteLine(x.CategoryName));
        }

        [Fact]
        public void DataConvertServiceTest()
        {
            var content = MockResponse();

            var result = _service.DeserializeBase(content);

            Console.WriteLine(result.Total);

            if (result != null)
            {
                Console.WriteLine(result.QihooApplist.Count);
            }
        }

        [Fact]
        public void FakeDataInsert()
        {
            var content = MockResponse();

            var result = _service.DeserializeBase(content);

            var reformApp = new ReformApp();
            foreach (var item in result.QihooApplist)
            {
                _cap.BuildAppProject(reformApp, item);
            }

            Console.WriteLine(reformApp.NewAppCount);
            Console.WriteLine(reformApp.NewVersionCount);
            Console.WriteLine(reformApp.DupVersionCount);
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
