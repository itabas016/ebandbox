using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Common;
using FrameMobile.Domain;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.Radar;
using FrameMobile.Web;
using StructureMap;
using NCore;

namespace Frame.Mobile.WebSite.Controllers
{
    [UserAuthorize(UserGroupTypes = "News")]
    public class RadarController : NewsBaseController
    {
        #region Prop

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
        public ActionResult RadarCategoryAdd(RadarCategory model, FormCollection parameters)
        {
            var exist = dbContextService.Exists<RadarCategory>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已存在！";
                return View();
            }

            var normallogoFile = Request.Files[Request.Files.Keys[0]];
            var hdlogoFile = Request.Files[Request.Files.Keys[1]];

            var normallogoFilePath = GetRadarCategoryLogoFilePath<RadarCategory>(model, normallogoFile);
            var hdlogoFilePath = GetRadarCategoryLogoFilePath<RadarCategory>(model, hdlogoFile);

            var logo_Image_Prefix = ConfigKeys.TYD_NEWS_RADAR_LOGO_IMAGE_PREFIX.ConfigValue();
            model.NormalLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(normallogoFilePath));
            model.HDLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(hdlogoFilePath));

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
        public ActionResult RadarCategoryEdit(RadarCategory model, HttpPostedFileBase normallogoFile, HttpPostedFileBase hdlogoFile)
        {
            var radarCategory = dbContextService.Single<RadarCategory>(model.Id);

            radarCategory.Name = model.Name;
            
            radarCategory.Comment = model.Comment;
            radarCategory.Status = model.Status;
            radarCategory.CreateDateTime = DateTime.Now;

            var normallogoFilePath = GetRadarCategoryLogoFilePath<RadarCategory>(model, normallogoFile);
            var hdlogoFilePath = GetRadarCategoryLogoFilePath<RadarCategory>(model, hdlogoFile);

            var logo_Image_Prefix = ConfigKeys.TYD_NEWS_RADAR_LOGO_IMAGE_PREFIX.ConfigValue();
            radarCategory.NormalLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(normallogoFilePath));
            radarCategory.HDLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(hdlogoFilePath));

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
            var radarcategorylist = RadarService.GetRadarCategoryList().ToList();

            ViewData["RadarCategorylist"] = radarcategorylist.GetSelectList();
            return View();
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarElementAdd(RadarElement model, FormCollection parameters)
        {
            var radarcategoryIds = parameters["radarcategory"].GetIds();

            var exist = dbContextService.Exists<RadarElement>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该元素已存在！";
                return View();
            }
            model.RadarCategoryIds = radarcategoryIds.GetString();
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
            var allradarcategorylist = RadarService.GetRadarCategoryList().ToList();
            var radarelement = dbContextService.Single<RadarElement>(radarElementId);

            var radarelementview = radarelement.To<RadarElementView>();
            ViewData["RadarCategorylist"] = allradarcategorylist.GetSelectList();
            ViewData["RadarCategoryIds"] = radarelementview.RadarCategoryIds;
            ViewData["IsUpdate"] = true;
            return View("RadarElementAdd", radarelementview);
        }

        [AdminAuthorize(UserGroups = "NewsAdministrator,NewsOperator")]
        [HttpPost]
        public ActionResult RadarElementEdit(RadarElement model, FormCollection parameters)
        {
            var radarcategoryIds = parameters["radarcategory"].GetIds();

            var radarElement = dbContextService.Single<RadarElement>(model.Id);

            radarElement.Name = model.Name;
            radarElement.RadarCategoryIds = radarcategoryIds.GetString();
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
