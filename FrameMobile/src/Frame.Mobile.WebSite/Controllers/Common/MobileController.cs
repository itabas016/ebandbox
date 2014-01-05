using FrameMobile.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frame.Mobile.WebSite.Controllers
{
    public class MobileController : MvcControllerBase
    {
        //
        // GET: /Common/

        public ActionResult Index()
        {
            return View();
        }

    }
}
