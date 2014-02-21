using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BaiduAppStoreCap;
using BaiduAppStoreCap.Model;
using BaiduAppStoreCap.Service;
using Xunit;

namespace FrameMobile.UnitTests.Tool.AppStores
{
    public class BaiduAppStoreCapTest
    {
        private DataConvertService _service;
        private AppItemCap _cap;

        public BaiduAppStoreCapTest()
        {
            _service = new DataConvertService();
            _cap = new AppItemCap();
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

        [Fact]
        public void AppDetailDeserializeTest()
        {
            var doc = _service.GetXmlDocument(MockAppDetailResponse());

            var model = _service.Deserialize<BaiduContentResult>(doc);

            Assert.Equal(0, model.StatusCode);
            Assert.Equal("疯狂捕鱼2", model.Result.AppDetail.Name);
        }

        [Fact]
        public void DownloadResourceTest()
        {
            var url = "http://cdn00.baidu-img.cn/timg?vsapp&size=b800_800&quality=100&imgtype=3&sec=1387443428&di=6c7ed63dd8583dc0e543dfe0558c384a&src=http%3A%2F%2Fc.hiphotos.bdimg.com%2Fwisegame%2Fpic%2Fitem%2Fb6628535e5dde711b28375e8a5efce1b9d16615e.jpg";

            var ret = _cap.GetDownloadUrl(url);
            Console.WriteLine(ret);
            var imageName = GetUrlName(ret);
            Console.WriteLine(imageName);
            //_cap.Download(url);
            //_cap.Download(redrict_url);
            var redirectUrl = "http://gdown.baidu.com/data/wisegame/4d91cdff7ca62946/fengkuangbuyu2duokuban_4.apk";
            var fileName = GetUrlName(redirectUrl);
            Console.WriteLine(fileName);
        }

        [Fact]
        public void GetRediectUrlTest()
        {
            var originalurl = "http://m.baidu.com/api?action=redirect&token=tianyida&from=1001816u&type=app&dltype=new&tj=game_3623078_1142824823_%E5%BC%80%E5%87%BA%E5%81%9C%E8%BD%A6%E5%9C%BA&blink=64c2687474703a2f2f61706b2e6766616e2e636f6d2f6261696475646f776e6c6f61642e7068703f61706b3d323330303732267372633d6261696475706167650653&crversion=1";

            var originalurl2 = "http://m.baidu.com/api?action=redirect&token=tianyida&from=1001816u&type=app&dltype=new&tj=game_3363485_3010049507_%E6%8B%AF%E6%95%91%E6%88%91%E7%9A%84%E7%94%B5%E8%A7%86&blink=aac7687474703a2f2f646f776e2e616e64726f69642e642e636e2f362f32322f383831365f3134313532303f687474703a2f2f632e646f776e616e64726f69642e636f6d2f616e64726f69642f6e65772f67616d65312f38372f3130313438372f7a6a776464735f313337313130363631323537342e61706b3f663d62616964755f310553&crversion=1";
            var appfileName = string.Empty;

            var ret = _cap.GetRedirectUrl(originalurl2, out appfileName);
            Console.WriteLine(appfileName);
        }

        private string MockAppDetailResponse()
        {
            var response = MockResponseBase("Files\\BaiduAppDetailResponse.txt");
            return response;
        }

        private string MockAppResponse()
        {
            var response = MockResponseBase("Files\\BaiduAppResponse.txt");
            return response;
        }

        private string MockBoardListResponse()
        {
            var response = MockResponseBase("Files\\BaiduBoardListResponse.txt");
            return response;
        }

        private string MockResponseBase(string path)
        {
            var response = string.Empty;
            using (var sr = new StreamReader(path))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }

        private string GetUrlName(string url)
        {
            Uri uri = new Uri(url);
            return uri.AbsolutePath.Replace("/", "_");
        }
    }
}
