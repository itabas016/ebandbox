using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
            var response = string.Empty;
            using (var sr = new StreamReader("Files\\TouTiaoResponse.txt"))
            {
                response = sr.ReadToEnd();
            }

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
                PublishTime = (float)1377039960.0,
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
            Assert.Equal(true, expect_cur.ContentList.Any(x => x.Id == result[0].Id));
        }
    }
}
