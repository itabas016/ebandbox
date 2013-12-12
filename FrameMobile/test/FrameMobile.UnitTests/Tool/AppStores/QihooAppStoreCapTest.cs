using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QihooAppStoreCap;
using QihooAppStoreCap.Model;
using QihooAppStoreCap.Service;
using Xunit;

namespace FrameMobile.UnitTests.Tool.AppStores
{
    public class QihooAppStoreCapTest
    {
        private DataConvertService _service;
        private AppItemCap _cap;
        private NewAppItemCap _newcap;


        public QihooAppStoreCapTest()
        {
            _service = new DataConvertService();
            _cap = new AppItemCap();
            _newcap = new NewAppItemCap();
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
        public void RequestCompleteTest()
        {
            var app = new GetApp();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["type"] = "1";
            var data = app.GetData(parameters, true);
            Console.WriteLine(data);
        }

        [Fact]
        public void CategoryTest()
        {
            var repo = new CategoryRepository();
            var ret = repo.GetAllCategory();
            ret.ForEach(x => Console.WriteLine(x.CategoryName));
            ret.ForEach(x => Console.WriteLine(x.CategoryPId));
            ret.ForEach(x => Console.WriteLine(x.CategoryId));
            ret.ForEach(x => Console.WriteLine(x.CategoryBrief));
            ret.ForEach(x => Console.WriteLine(x.CategoryBanner1));
            ret.ForEach(x => Console.WriteLine(x.CategoryBanner2));
        }

        [Fact]
        public void DataConvertServiceTest()
        {
            var content = MockResponse();

            var result = _service.DeserializeBase<QihooAppStoreGetAppResult>(content);

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

            var result = _service.DeserializeBase<QihooAppStoreGetAppResult>(content);

            var reformApp = new ReformApp();
            foreach (var item in result.QihooApplist)
            {
                _cap.BuildAppProject(reformApp, item);
            }

            Console.WriteLine(reformApp.NewAppCount);
            Console.WriteLine(reformApp.NewVersionCount);
            Console.WriteLine(reformApp.DupVersionCount);
        }

        [Fact]
        public void FakeDataInsert2()
        {
            var content = MockResponseComplete();

            var result = _service.DeserializeBase<QihooAppStoreGetCompleteAppResult>(content);

            var reformApp = new ReformApp();
            foreach (var item in result.QihooApplist)
            {
                _newcap.BuildAppProject(reformApp, item);
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

        private string MockResponseComplete()
        {
            var response = string.Empty;
            using (var sr = new StreamReader("Files\\QihooGetCompleteAppResponse.txt"))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }
    }
}
