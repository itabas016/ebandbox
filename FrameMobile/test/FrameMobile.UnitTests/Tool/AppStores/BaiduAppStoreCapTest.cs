using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BaiduAppStoreCap.Model;
using BaiduAppStoreCap.Service;
using Xunit;

namespace FrameMobile.UnitTests.Tool.AppStores
{
    public class BaiduAppStoreCapTest
    {
        private DataConvertService _service;

        public BaiduAppStoreCapTest()
        {
            _service = new DataConvertService();
        }

        [Fact]
        public void GetXmlDocumentTest()
        {
            var doc = _service.GetXmlDocument(MockBoardListResponse());

            var model = _service.Deserialize<BaiduBoardListResult>(doc);

            Assert.Equal(0, model.StatusCode);
            Assert.Equal(6, model.BoardList.Count);
            Assert.NotEqual(true, string.IsNullOrEmpty(model.BoardList[0].Id.ToString()));

            var doc_app = _service.GetXmlDocument(MockAppResponse());

            var model2 = _service.Deserialize<BaiduBoardResult>(doc_app);

            Assert.Equal(0, model2.StatusCode);
            Assert.Equal(3, model2.Result.AppList.Count);
            Assert.NotEqual(true, string.IsNullOrEmpty(model2.Result.AppList[0].Name));
        }

        private string MockAppResponse()
        {
            var response = string.Empty;
            using (var sr = new StreamReader("Files\\BaiduAppResponse.txt"))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }

        private string MockBoardListResponse()
        {
            var response = string.Empty;
            using (var sr = new StreamReader("Files\\BaiduBoardListResponse.txt"))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }
    }
}
