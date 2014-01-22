using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Model.Theme;
using FrameMobile.Domain;

namespace Frame.Mobile.WebSite.Controllers.Theme
{
    public class ThemeUIController : ThemeBaseController
    {
        protected override bool IsMobileInterface { get { return false; } }

        #region Config

        public ActionResult ConfigManage()
        {
            return RedirectToAction("ConfigList");
        }

        public ActionResult ConfigList()
        {
            var configlist = dbContextService.All<ThemeConfig>().ToList();
            ViewData["Configlist"] = configlist;
            ViewData["TotalCount"] = configlist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult ConfigAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfigAdd(ThemeConfig model)
        {
            var exist = dbContextService.Exists<ThemeConfig>(x => x.NameLowCase == model.NameLowCase);
            if (exist)
            {
                TempData["errorMsg"] = "该配置表已经存在！";
                return View();
            }
            var ret = dbContextService.Add<ThemeConfig>(model);

            return RedirectToAction("ConfigList");
        }

        [HttpGet]
        public ActionResult ConfigEdit(int configId)
        {
            var config = dbContextService.Single<ThemeConfig>(configId);
            ViewData["IsUpdate"] = true;
            return View("ConfigAdd", config);
        }

        [HttpPost]
        public ActionResult ConfigEdit(ThemeConfig model)
        {
            var config = dbContextService.Single<ThemeConfig>(model.Id);

            config.Name = model.Name;
            config.NameLowCase = config.NameLowCase;
            config.Status = model.Status;
            config.Type = model.Type;
            config.Comment = model.Comment;
            config.CreateDateTime = DateTime.Now;

            dbContextService.Update<ThemeConfig>(config);
            return RedirectToAction("ConfigList");
        }

        public ActionResult ConfigDelete(int configId)
        {
            var ret = dbContextService.Delete<ThemeConfig>(configId);
            return RedirectToAction("ConfigList");
        }

        #endregion
    }
}
