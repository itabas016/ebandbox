using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model.News;
using FrameMobile.Web;
using StructureMap;

namespace Frame.Mobile.WebSite.Controllers
{
    public class NewsUIController : MvcControllerBase
    {
        #region Prop

        private INewsUIService _newsUIService;
        public INewsUIService NewsUIService
        {
            get
            {
                if (_newsUIService == null)
                {
                    _newsUIService = ObjectFactory.GetInstance<INewsUIService>();
                }
                return _newsUIService;
            }
            set
            {
                _newsUIService = value;
            }
        }

        private IDbContextService _dbContextService;
        public IDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<IDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        protected override bool IsMobileInterface { get { return false; } }

        #endregion

        #region Ctor

        public NewsUIController(INewsUIService newsUIService)
        {
            this.NewsUIService = newsUIService;
        }

        #endregion

        #region News

        public ActionResult NewsManage()
        {
            return View();
        }

        #endregion

        #region NewsConfig

        public ActionResult ConfigManage()
        {
            var configlist = dbContextService.All<NewsConfig>();
            return View();
        }

        [HttpGet]
        public ActionResult AddConfig()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddConfig(NewsConfig model)
        {
            var exist = dbContextService.Exists<NewsConfig>(x=>x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该配置项已存在！";
                return View();
            }
            var ret = dbContextService.Add<NewsConfig>(model);
            return RedirectToAction("ConfigManage");
        }

        public ActionResult DeleteConfig(NewsConfig model)
        {
            var ret = dbContextService.Delete<NewsConfig>(model);
            return View();
        }

        #endregion

        #region NewsSource

        public ActionResult Source()
        {
            return View();
        }

        public ActionResult AddSource()
        {
            return View();
        }

        public ActionResult DeleteSource()
        {
            return View();
        }

        #endregion

        #region NewsCategory

        public ActionResult Category()
        {
            return View();
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        public ActionResult DeleteCategory()
        {
            return View();
        }

        #endregion

        #region NewsExtraApp

        public ActionResult ExtraApp()
        {
            return View();
        }

        public ActionResult AddExtraApp()
        {
            return View();
        }

        public ActionResult DeleteExtraApp()
        {
            return View();
        }

        #endregion

        #region Helper

        #endregion
    }
}
