using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Cache;
using FrameMobile.Common;
using StructureMap;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;

namespace Frame.Mobile.WebSite.Controllers
{
    public class CacheController : Controller
    {
        public ICacheService CacheService
        {
            get
            {
                if (_cacheService == null)
                {
                    _cacheService = ObjectFactory.GetInstance<ICacheService>();
                }
                return _cacheService;
            }
            set { _cacheService = value; }
        }
        private ICacheService _cacheService;

        public ActionResult NewsClear()
        {
            var newscacheService = RedisClientManagerType.NewsCache.RedisCacheServiceFactory();

            CacheService.Clear(newscacheService);

            return Content("news service cache cleared.");
        }

        public ActionResult ThemeClear()
        {
            var themecacheService = RedisClientManagerType.ThemeCache.RedisCacheServiceFactory();

            CacheService.Clear(themecacheService);

            return Content("theme service cache cleared.");
        }

        public ActionResult Clear()
        {
            var commoncacheService = RedisClientManagerType.MixedCache.RedisCacheServiceFactory();

            CacheService.Clear(commoncacheService);

            return Content("common service cache cleared.");
        }

        public ActionResult ClearAll()
        {
            var commoncacheService = RedisClientManagerType.MixedCache.RedisCacheServiceFactory();

            CacheService.ClearAll(commoncacheService);

            return Content("common service cache cleared.");
        }

    }
}
