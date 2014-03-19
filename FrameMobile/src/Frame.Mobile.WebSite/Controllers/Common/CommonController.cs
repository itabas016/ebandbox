using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain;
using FrameMobile.Web;

namespace Frame.Mobile.WebSite.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult ModelSync()
        {
            try
            {
                DBModelstrapper.Initialize();
                return Content("model sync db success!");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}
