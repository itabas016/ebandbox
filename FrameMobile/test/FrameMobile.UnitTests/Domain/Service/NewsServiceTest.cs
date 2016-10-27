using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using Moq;
using StructureMap;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class NewsServiceTest : TestBase
    {
        public INewsService newsService
        {
            get
            {
                if (_newsService == null)
                {
                    _newsService = ObjectFactory.GetInstance<INewsService>();
                }
                return _newsService;
            }
        }private INewsService _newsService;

        IRequestRepository requestRepo;
        Mock<IRequestRepository> _requestRepoMock;

        public NewsServiceTest()
        {
            _requestRepoMock = new Mock<IRequestRepository>();
            requestRepo = _requestRepoMock.Object;
        }
        [Fact(Skip = "MySqlSelect")]
        public void NewsListTest()
        {
            var namevalues = new NameValueCollection();
            namevalues[MobileParam.Key_Resolution] = "800x600";
            namevalues[MobileParam.Key_IMSI] = "11111";
            namevalues["format"] = "json";
            _requestRepoMock.Setup<NameValueCollection>(m => m.Header).Returns(namevalues);
            MobileParam mobileParam = new MobileParam(requestRepo);

            var newsId = 1;
            //获取最新
            var action = true;
            var categoryIds = "1;2";
            var totalCount = 0;

            var result = newsService.GetNewsContentViewList(mobileParam, newsId, action, categoryIds, 1, 5, out totalCount);
            foreach (var item in result)
            {
                Console.WriteLine(item.NewsId);
            }
        }

        public void test()
        {
            var s = "http://news.kk874.com/NewsResources/Images/480/2801491477_large_325_681220495.jpg";
            var ret = s.Split('/').ToList();
            Console.WriteLine(ret[ret.Count-1]);
            ret.ForEach(m => Console.WriteLine(m));
        }
    }
}
