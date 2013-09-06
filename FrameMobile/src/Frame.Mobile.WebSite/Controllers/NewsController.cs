using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain.Service;
using FrameMobile.Web;
using StructureMap;

namespace Frame.Mobile.WebSite.Controllers
{
    public class NewsController : MvcControllerBase
    {
        public INewsService NewsService 
        {
            get
            {
                if (_newsService == null)
                    _newsService = ObjectFactory.GetInstance<INewsService>();

                return _newsService;
            }
            set
            {
                _newsService = value;
            }
        }
        private INewsService _newsService;

        public ActionResult Category(string imsi)
        {
            return Content("");
        }

        public ActionResult SubCategory(string imsi)
        {
            return Content("");
        }

        public ActionResult NewsList(string imsi, string category)
        {
            return Content("");
        }
    }
}
