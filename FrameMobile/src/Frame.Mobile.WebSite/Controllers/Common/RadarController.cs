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

        #region Radar

        public ActionResult RadarList()
        {
            var radarlist = dbContextService.All<Radar>().ToList();
            ViewData["radarlist"] = radarlist;
            ViewData["TotalCount"] = radarlist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult RadarAdd()
        {
            return View();
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarAdd(Radar model)
        {
            var exist = dbContextService.Exists<Radar>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已存在！";
                return View();
            }
            var ret = dbContextService.Add<Radar>(model);

            RadarService.UpdateServerVersion<Radar>();

            return RedirectToAction("RadarList");
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        public ActionResult RadarDelete(int RadarId)
        {
            var ret = dbContextService.Delete<Radar>(RadarId);

            RadarService.UpdateServerVersion<Radar>();

            return RedirectToAction("RadarList");
        }

        [HttpGet]
        public ActionResult RadarEdit(int RadarId)
        {
            var radar = dbContextService.Single<Radar>(RadarId);
            ViewData["IsUpdate"] = true;
            return View("RadarAdd", radar);
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarEdit(Radar model)
        {
            var radar = dbContextService.Single<Radar>(model.Id);

            radar.Name = model.Name;
            radar.Comment = model.Comment;
            radar.Status = model.Status;
            radar.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<Radar>(radar);

            RadarService.UpdateServerVersion<Radar>();

            return RedirectToAction("RadarList");
        }

        #endregion

        #region SubRadar

        public ActionResult SubRadarList()
        {
            var subradarlist = dbContextService.All<SubRadar>().ToList();
            ViewData["subradarlist"] = subradarlist;
            ViewData["TotalCount"] = subradarlist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult SubRadarAdd()
        {
            var radarlist = RadarService.GetRadarList().ToList();

            ViewData["Radarlist"] = radarlist.GetSelectList();
            return View();
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult SubRadarAdd(SubRadar model)
        {
            var exist = dbContextService.Exists<SubRadar>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已存在！";
                return View();
            }
            var ret = dbContextService.Add<SubRadar>(model);

            RadarService.UpdateServerVersion<SubRadar>();

            return RedirectToAction("SubRadarList");
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        public ActionResult SubRadarDelete(int subRadarId)
        {
            var ret = dbContextService.Delete<SubRadar>(subRadarId);

            RadarService.UpdateServerVersion<SubRadar>();

            return RedirectToAction("SubRadarList");
        }

        [HttpGet]
        public ActionResult SubRadarEdit(int SubRadarId)
        {
            var radarlist = RadarService.GetRadarList().ToList();
            var subradar = dbContextService.Single<SubRadar>(SubRadarId);

            ViewData["Radarlist"] = radarlist.GetSelectList(subradar.RadarId);
            ViewData["IsUpdate"] = true;
            return View("SubRadarAdd", subradar);
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult SubRadarEdit(SubRadar model)
        {
            var subradar = dbContextService.Single<SubRadar>(model.Id);

            subradar.Name = model.Name;
            subradar.RadarId = model.RadarId;
            subradar.Comment = model.Comment;
            subradar.Status = model.Status;
            subradar.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<SubRadar>(subradar);

            RadarService.UpdateServerVersion<SubRadar>();

            return RedirectToAction("SubRadarList");
        }

        #endregion
    }
}
