using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using FrameMobile.Model.ThirdPart;
using Xunit;

namespace FrameMobile.UnitTests.Domain
{
    public class FetchTouTiaoServiceTest
    {
        FetchTouTiaoService service;

        public FetchTouTiaoServiceTest()
        {
            service = new FetchTouTiaoService();
        }

        [Fact]
        public void GenerateParameterTest()
        {
            var result = service.GenerateParam();
            var secureKey = "5a74f9188e0e413e28fe3c490b009ce7";
            var partner = "tydtech";
            Assert.Equal(partner, result.Partner);
            Assert.Equal(secureKey, result.SecureKey);
            Assert.Equal(true, result.Nonce > 0 && result.Nonce <= 10000 ? true : false);

            Console.WriteLine(result.Timestamp);
            Console.WriteLine(result.Signature);
        }

        public void RequestTest()
        {
            var category = "news_hot";
            var response = service.Request(category);
            Console.WriteLine(response);
        }

        [Fact]
        public void AnlynazeTest()
        {
            /*
            {"msg": "OK", "data": {"cursor": 1376933340, "data": [{"app_open_url": "snssdk143://detail?groupid=2385553034",
             "publish_time": 1377039960.0, "toutiao_url": "http://www.toutiao.com/i/group/article/2385553034/",
             "toutiao_wap_url": "http://m.toutiao.com/group/article/2385553034/", "bury_count": 2,
             "title": "4\u540d\u5fd7", "tip": 0, "site": "\u8fbd\u5b81\u9891\u9053",
             "site_url": "http://liaoning.nen.com.cn/system/2013/08/21/010723716.shtml",
             "comment_count": 3, "images": [{"width": 410, "urls": ["http://p0.pstatp.com/origin/252/5327833163",
             "http://p.pstatp.com/origin/252/5327833163"], "height": 283}], "digg_count": 2, "group_id": 2385553034,
             "id": 2385553034, "favorite_count": 1},{"app_open_url": "snssdk143://detail?groupid=2348076531",
             "publish_time": 1376928000.0, "toutiao_url": "http://www.toutiao.com/i/group/article/2348076531/",
             "toutiao_wap_url": "http://m.toutiao.com/group/article/2348076531/", "bury_count": 1,
             "title": "\u5f20\u7d20", "tip": 0, "site": "\u5609\u5174\u65e5\u62a5",
             "site_url": "http://jxrb.cnjxol.com/html/2013-08/20/content_658845.htm", "comment_count": 6,
             "images": [], "digg_count": 33, "group_id": 2348076531, "id": 2348076531, "favorite_count": 19}]}, "ret": 0}
             */
            var response = "{\"msg\": \"OK\", \"data\": {\"cursor\": 1376933340, \"data\": [{\"app_open_url\": \"snssdk143://detail?groupid=2385553034\", \"publish_time\": 1377039960.0, \"toutiao_url\": \"http://www.toutiao.com/i/group/article/2385553034/\", \"toutiao_wap_url\": \"http://m.toutiao.com/group/article/2385553034/\", \"bury_count\": 2, \"title\": \"4\u540d\u5fd7\", \"tip\": 0, \"site\": \"\u8fbd\u5b81\u9891\u9053\", \"site_url\": \"http://liaoning.nen.com.cn/system/2013/08/21/010723716.shtml\", \"comment_count\": 3, \"images\": [{\"width\": 410, \"urls\": [\"http://p0.pstatp.com/origin/252/5327833163\", \"http://p.pstatp.com/origin/252/5327833163\"], \"height\": 283}], \"digg_count\": 2, \"group_id\": 2385553034, \"id\": 2385553034, \"favorite_count\": 1},{\"app_open_url\": \"snssdk143://detail?groupid=2348076531\", \"publish_time\": 1376928000.0, \"toutiao_url\": \"http://www.toutiao.com/i/group/article/2348076531/\", \"toutiao_wap_url\": \"http://m.toutiao.com/group/article/2348076531/\", \"bury_count\": 1, \"title\": \"\u5f20\u7d20\", \"tip\": 0, \"site\": \"\u5609\u5174\u65e5\u62a5\", \"site_url\": \"http://jxrb.cnjxol.com/html/2013-08/20/content_658845.htm\", \"comment_count\": 6, \"images\": [], \"digg_count\": 33, \"group_id\": 2348076531, \"id\": 2348076531, \"favorite_count\": 19}]}, \"ret\": 0}";

            var result = service.Anlynaze(response);

            #region Expect value

            var image1 = new TouTiaoImageInfo()
            {
                Width = 410,
                Height = 283,
                UrlList = new List<string>() { "http://p0.pstatp.com/origin/252/5327833163",
                    "http://p.pstatp.com/origin/252/5327833163" },
            };

            var image2 = new TouTiaoImageInfo()
            {
            };

            var content1 = new TouTiaoContent()
            {
                AppOpeURL = "snssdk143://detail?groupid=2385553034",
                PublishTime = (int)1377039960.0,
                Id = 2385553034,
                Title = "4\u540d\u5fd7",
                ImageList = new List<TouTiaoImageInfo>() { image1, image2 },
            };

            var content2 = new TouTiaoContent()
            {
                Id = 2348076531,
                FavoriteCount = 19,
                GroupId = 2348076531
            };

            var expect_cur = new TouTiaoCursor()
                {
                    Cursor = 1376933340,
                    ContentList = new List<TouTiaoContent>() { content1, content2 }
                };
            var expect_ret = new TouTiaoResult()
            {
                ret = 0,
                Msg = "OK",
                DataByCursor = expect_cur
            };

            #endregion

            Assert.Equal(result.Count, 2);
        }
    }
}
