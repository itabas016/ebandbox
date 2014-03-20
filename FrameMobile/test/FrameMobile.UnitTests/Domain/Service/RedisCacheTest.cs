using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Cache;
using StructureMap;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class RedisCacheTest : TestBase
    {
        public IRedisCacheService redisCacheService
        {
            get
            {
                if (_redisCacheService == null)
                {
                    _redisCacheService = ObjectFactory.GetInstance<IRedisCacheService>();
                }
                return _redisCacheService;
            }
        }private IRedisCacheService _redisCacheService;

        [Fact]
        public void SetNXTest()
        {
            var key = "key:a:LOCK";
            var ret = redisCacheService.SetNX(key);
            redisCacheService.Add(key, "dsss");
            var value = redisCacheService.GetData<string>(key);
            Assert.Equal(key,value);
        }
    }
}
