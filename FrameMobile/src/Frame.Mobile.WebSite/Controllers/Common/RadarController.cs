using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.Radar;
using FrameMobile.Web;
using StructureMap;

namespace Frame.Mobile.WebSite.Controllers
{
    [UserAuthorize(UserGroupTypes = "News")]
    public class RadarController : MvcControllerBase
    {
        #region Prop

        private IRadarService _radarService;
        public IRadarService RadarService
        {
            get
            {
                if (_radarService == null)
                {
                    _radarService = ObjectFactory.GetInstance<IRadarService>();
                }
                return _radarService;
            }
            set
            {
                _radarService = value;
            }
        }

        private INewsDbContextService _dbContextService;
        public INewsDbContextService dbContextService
        {
            get
            {
                if (_dbContextService == null)
                {
                    _dbContextService = ObjectFactory.GetInstance<INewsDbContextService>();
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

        #region RadarCategory

        public ActionResult RadarCategoryList()
        {
            var radarcategorylist = dbContextService.All<RadarCategory>().ToList();
            ViewData["radarCategorylist"] = radarcategorylist;
            ViewData["TotalCount"] = radarcategorylist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult RadarCategoryAdd()
        {
            return View();
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarCategoryAdd(RadarCategory model)
        {
            var exist = dbContextService.Exists<RadarCategory>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已存在！";
                return View();
            }
            var ret = dbContextService.Add<RadarCategory>(model);

            RadarService.UpdateServerVersion<RadarCategory>();

            return RedirectToAction("RadarCategoryList");
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        public ActionResult RadarCategoryDelete(int radarCategoryId)
        {
            var ret = dbContextService.Delete<RadarCategory>(radarCategoryId);

            RadarService.UpdateServerVersion<RadarCategory>();

            return RedirectToAction("RadarCategoryList");
        }

        [HttpGet]
        public ActionResult RadarCategoryEdit(int radarCategoryId)
        {
            var radarCategory = dbContextService.Single<RadarCategory>(radarCategoryId);
            ViewData["IsUpdate"] = true;
            return View("RadarCategoryAdd", radarCategory);
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarCategoryEdit(RadarCategory model)
        {
            var radarCategory = dbContextService.Single<RadarCategory>(model.Id);

            radarCategory.Name = model.Name;
            radarCategory.Comment = model.Comment;
            radarCategory.Status = model.Status;
            radarCategory.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<RadarCategory>(radarCategory);

            RadarService.UpdateServerVersion<RadarCategory>();

            return RedirectToAction("RadarCategoryList");
        }

        #endregion

        #region RadarElement

        public ActionResult RadarElementList()
        {
            var radarelementlist = dbContextService.All<RadarElement>().ToList();
            ViewData["radarelementlist"] = radarelementlist;
            ViewData["TotalCount"] = radarelementlist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult RadarElementAdd()
        {
            var radarlist = RadarService.GetRadarList().ToList();

            ViewData["RadarCategorylist"] = radarlist.GetSelectList();
            return View();
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarElementAdd(RadarElement model)
        {
            var exist = dbContextService.Exists<RadarElement>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该元素已存在！";
                return View();
            }
            var ret = dbContextService.Add<RadarElement>(model);

            RadarService.UpdateServerVersion<RadarElement>();

            return RedirectToAction("RadarElementList");
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        public ActionResult RadarElementDelete(int radarElementId)
        {
            var ret = dbContextService.Delete<RadarElement>(radarElementId);

            RadarService.UpdateServerVersion<RadarElement>();

            return RedirectToAction("RadarElementList");
        }

        [HttpGet]
        public ActionResult RadarElementEdit(int radarElementId)
        {
            var radarlist = RadarService.GetRadarList().ToList();
            var subradar = dbContextService.Single<RadarElement>(radarElementId);

            ViewData["RadarCategorylist"] = radarlist.GetSelectList(subradar.RadarId);
            ViewData["IsUpdate"] = true;
            return View("RadarElementAdd", subradar);
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarElementEdit(RadarElement model)
        {
            var radarElement = dbContextService.Single<RadarElement>(model.Id);

            radarElement.Name = model.Name;
            radarElement.RadarId = model.RadarId;
            radarElement.Comment = model.Comment;
            radarElement.Status = model.Status;
            radarElement.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<RadarElement>(radarElement);

            RadarService.UpdateServerVersion<RadarElement>();

            return RedirectToAction("RadarElementList");
        }

        #endregion
    }
}
