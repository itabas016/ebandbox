using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Cache;

namespace FrameMobile.Domain.Service
{
    public interface ICacheService
    {
        void Clear(IRedisCacheService cacheService);
        void ClearAll(IRedisCacheService cacheService);
    }
}
