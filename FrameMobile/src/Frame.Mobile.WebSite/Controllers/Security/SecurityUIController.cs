using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model.Security;
using FrameMobile.Web;
using StructureMap;

namespace Frame.Mobile.WebSite.Controllers
{
    public class SecurityUIController : MvcControllerBase
    {
        #region Prop

        protected override bool IsMobileInterface { get { return false; } }

        #endregion

        private ISecurityDbContextService _dbContextService;
        public ISecurityDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<ISecurityDbContextService>();
                }
                return _dbContextService;
            }
            set
            {
                _dbContextService = value;
            }
        }

        public ActionResult FloatingList()
        {
            var floatinglist = dbContextService.All<FloatingModel>().ToList();
            ViewData["Floatinglist"] = floatinglist;
            ViewData["TotalCount"] = floatinglist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult FloatingAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FloatingAdd(FloatingModel model)
        {
            var ret = dbContextService.Add<FloatingModel>(model);

            return RedirectToAction("FloatingList");
        }

        [HttpGet]
        public ActionResult FloatingEdit(int floatingId)
        {
            var Floating = dbContextService.Single<FloatingModel>(floatingId);
            ViewData["IsUpdate"] = true;
            return View("FloatingAdd", Floating);
        }

        [HttpPost]
        public ActionResult FloatingEdit(FloatingModel model)
        {
            var floating = dbContextService.Single<FloatingModel>(model.Id);

            floating.JsonResult = model.JsonResult;

            floating.Version = floating.JsonResult == model.JsonResult ? floating.Version : floating.Version + 1;
            floating.Status = model.Status;
            floating.CreateDateTime = DateTime.Now;

            dbContextService.Update<FloatingModel>(floating);
            return RedirectToAction("FloatingList");
        }

        public ActionResult FloatingDelete(int floatingId)
        {
            var ret = dbContextService.Delete<FloatingModel>(floatingId);
            return RedirectToAction("FloatingList");
        }

        public ActionResult ConfigList()
        {
            var configlist = dbContextService.All<SecurityConfig>().ToList();
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
        public ActionResult ConfigAdd(SecurityConfig model)
        {
            var exist = dbContextService.Exists<SecurityConfig>(x => x.Type == model.Type);
            if (exist)
            {
                TempData["errorMsg"] = "该配置已经存在！";
                return View();
            }

            var ret = dbContextService.Add<SecurityConfig>(model);

            return RedirectToAction("ConfigList");
        }

        [HttpGet]
        public ActionResult ConfigEdit(int configId)
        {
            var Config = dbContextService.Single<SecurityConfig>(configId);
            ViewData["IsUpdate"] = true;
            return View("ConfigAdd", Config);
        }

        [HttpPost]
        public ActionResult ConfigEdit(SecurityConfig model)
        {
            var config = dbContextService.Single<SecurityConfig>(model.Id);

            config.Name = model.Name;
            config.Type = model.Type;
            config.LatestVersion = model.LatestVersion;
            config.Rate = model.Rate;
            config.Status = model.Status;
            config.CreateDateTime = DateTime.Now;

            dbContextService.Update<SecurityConfig>(config);
            return RedirectToAction("ConfigList");
        }

        public ActionResult ConfigDelete(int configId)
        {
            var ret = dbContextService.Delete<SecurityConfig>(configId);
            return RedirectToAction("ConfigList");
        }

        #region Helper

        public void UpdateSecurityConfigVersion()
        {
        }

        #endregion
    }
}
