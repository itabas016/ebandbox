using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FrameMobile.Domain.Service;
using StructureMap;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class TencentServiceTest : TestBase
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

        protected TencentService service;

        public TencentServiceTest()
        {
            service = new TencentService(_dbContextService);
        }

        [Fact]
        public void TencentListTest()
        {
            var response = GetMockResponse();

            var ret = service.GetContentlist(response);

            Assert.Equal(1, ret.Count);
            Assert.Equal("NEW2013122001007400", ret[0].NewsId);
        }

        private string GetMockResponse()
        {
            var response = string.Empty;
            using (var sr = new StreamReader("Files//TencentNewsResponse.txt"))
            {
                response = sr.ReadToEnd();
            }
            return response;
        }
    }
}
