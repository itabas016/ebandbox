﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FrameMobile.Domain.Service;
using FrameMobile.Model.ThirdPart;
using Xunit;
using FrameMobile.Domain;
using Moq;
using StructureMap;
using FrameMobile.Model.News;

namespace FrameMobile.UnitTests.Domain
{
    public class TouTiaoServiceTest : TestBase
    {
        public INewsDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
                }
                return _dbContextService;
            }
        }
        private INewsDbContextService _dbContextService;

        protected TouTiaoService dataService;

        public TouTiaoServiceTest()
        {
            dataService = new TouTiaoService(_dbContextService);
        }

        [Fact(Skip = "TouTiaoSecret")]
        public void GenerateParameterTest()
        {
            var result = dataService.GenerateParam();
            var secureKey = "5a74f9188e0e413e28fe3c490b009ce7";
            var partner = "tydtech";
            Assert.Equal(partner, result.Partner);
            Assert.Equal(secureKey, result.SecureKey);
            Assert.Equal(true, result.Nonce > 0 && result.Nonce <= 10000 ? true : false);

            Console.WriteLine(result.Timestamp);
            Console.WriteLine(result.Signature);
        }

        [Fact(Skip = "TouTiaoRequest")]
        public void RequestTest()
        {
            var categoryList = Enum.GetNames(typeof(TouTiaoCategory));

            foreach (var item in categoryList)
            {
                var response = dataService.Request(item);
                var instance = dataService.DeserializeTouTiao(response);

                Assert.Equal(0, instance.ret);
                Assert.NotEqual(null, instance.DataByCursor);
                var contentList = instance.DataByCursor.ContentList;
                if (contentList.Count > 0)
                {
                    Console.WriteLine(string.Format("{0}:{1} 游标{2}", item, contentList.Count, instance.DataByCursor.Cursor));
                }
            }

        }

        [Fact]
        public void DeserializeTouTiaoTest()
        {
            var response = GetMockResponse();
            var result = dataService.DeserializeTouTiao(response);

            var expect_ret = new TouTiaoResult()
            {
                ret = 0,
                Msg = "OK",
                DataByCursor = null
            };

            Assert.Equal(expect_ret.Msg, result.Msg);
            Assert.Equal(expect_ret.ret, result.ret);
            Assert.Equal(result.DataByCursor.ContentList.Count, 2);

        }

        [Fact]
        public void AnlynazeTest()
        {
            var resonse = GetMockResponse();
            var toutiaoResult = dataService.DeserializeTouTiao(resonse);

            long cursor = 0;
            var instance = dataService.Anlynaze(toutiaoResult, out cursor);

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
                PublishTime = (long)1377039960.0,
                NewsId = 2385553034,
                Title = "4\u540d\u5fd7",
                ImageList = new List<TouTiaoImageInfo>() { image1, image2 },
            };

            var content2 = new TouTiaoContent()
            {
                NewsId = 2348076531,
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

            Assert.Equal(cursor, 1376933340);
            Assert.Equal(instance.Count, 2);
            Assert.Equal(true, expect_cur.ContentList.Any(x => x.NewsId == instance[0].NewsId));
        }

        [Fact(Skip = "TouTiaoRequest")]
        public void AnlynazeNoRepeatTest()
        {
            long cursor = 1379851192;
            var response = dataService.Request("news_hot", cursor, 50);
            var toutiaoResult = dataService.DeserializeTouTiao(response);

            var result = dataService.Anlynaze(toutiaoResult, out cursor);
            Console.WriteLine(result.Count);
            Console.WriteLine(cursor);
            var sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append(item.NewsId + " " + item.PublishTime.UTCStamp().ToString() + "\r");
            }
            Console.WriteLine(sb.ToString());
        }

        [Fact]
        public void Anlynaze_Error_Msg_Test()
        {
            var response = "\"msg\":REGDRET,\"ret\":1";
        }

        [Fact(Skip = "MySQLInsert")]
        public void SingleCaptureTest()
        {
            var categoryName = "news_tech";

            dataService.SingleCapture(categoryName);
        }

        [Fact(Skip = "MySQLInsert")]
        public void CaptureTest()
        {
            dataService.Capture();
        }

        [Fact(Skip = "MySQLInsert")]
        public void SingleImageSave()
        {
            long newsId = 32434321;
            var HDURL = string.Empty;
            var NormalURL = string.Empty;
            var image = new TouTiaoImageInfo()
            {
                Height = 200,
                Width = 150,
                UrlList = new List<string>() { "http://p0.pstatp.com/origin/252/6926772543" }
            };

            dataService.SingleImageSave(image, newsId, out HDURL, out NormalURL);
        }

        #region Helper

        private string GetMockResponse()
        {
            var response = string.Empty;
            using (var sr = new StreamReader("Files//TouTiaoResponse.txt"))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }

        #endregion
    }
}
