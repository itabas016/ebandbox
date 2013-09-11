using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Cache;
using StructureMap;

namespace Frame.Mobile.WebSite.Controllers
{
    public class CacheController : Controller
    {
        public ICacheManagerHelper CacheService
        {
            get
            {
                if (_cacheService == null)
                {
                    _cacheService = ObjectFactory.GetInstance<ICacheManagerHelper>();
                }
                return _cacheService;
            }
            set { _cacheService = value; }
        }
        private ICacheManagerHelper _cacheService;

        public CacheController(ICacheManagerHelper cacheService)
        {
            this.CacheService = cacheService;
        }

        public ActionResult ClearService()
        {
            CacheService.ClearServiceCache();
            List<string> toRemove = new List<string>();
            foreach (DictionaryEntry cacheItem in HttpRuntime.Cache)
            {
                toRemove.Add(cacheItem.Key.ToString());
            }
            foreach (string key in toRemove)
            {
                HttpRuntime.Cache.Remove(key);
            }

            return Content("service cache cleared.");
        }

        public ActionResult ClearAll()
        {
            CacheService.Flush();
            return Content("all cache cleared.");
        }

    }
}
