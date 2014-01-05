using FrameMobile.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame.Mobile.WebSite.Controllers
{
    public class WallPaperUIController : ThemeBaseController
    {
        protected override bool IsMobileInterface { get { return false; } }

        public ActionResult Index()
        {
            return View();
        }

    }
}
