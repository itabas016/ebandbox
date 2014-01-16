using FrameMobile.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Domain;
using FrameMobile.Model.Mobile;
using StructureMap;
using FrameMobile.Domain.Service;

namespace Frame.Mobile.WebSite.Controllers
{
    public class MobileController : ThemeBaseController
    {
        protected override bool IsMobileInterface { get { return false; } }

        #region Property

        public ActionResult PropertyList()
        {
            var propertylist = MobileUIService.GetMobilePropertyList();
            ViewData["Propertylist"] = propertylist;
            ViewData["TotalCount"] = propertylist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult PropertyAdd()
        {
            var brandlist = MobileUIService.GetMobileBrandList();
            var hardwarelist = MobileUIService.GetMobileHardwareList();
            var resolutionlist = MobileUIService.GetMobileResolutionList();
            ViewData["Brandlist"] = brandlist.GetSelectList();
            ViewData["Hardwarelist"] = hardwarelist.GetSelectList();
            ViewData["Resolutionlist"] = resolutionlist.GetSelectList();

            return View();
        }

        [HttpPost]
        public ActionResult PropertyAdd(MobileProperty model)
        {
            var exist = dbContextService.Exists<MobileProperty>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该关联配置已经存在！";
                return View();
            }
            var ret = dbContextService.Add<MobileProperty>(model);
            return RedirectToAction("PropertyList");
        }

        [HttpGet]
        public ActionResult PropertyEdit(int propertyId)
        {
            var brandlist = MobileUIService.GetMobileBrandList();
            var hardwarelist = MobileUIService.GetMobileHardwareList();
            var resolutionlist = MobileUIService.GetMobileResolutionList();
            ViewData["Brandlist"] = brandlist.GetSelectList();
            ViewData["Hardwarelist"] = hardwarelist.GetSelectList();
            ViewData["Resolutionlist"] = resolutionlist.GetSelectList();

            var property = dbContextService.Single<MobileProperty>(propertyId);
            ViewData["IsUpdate"] = true;
            return View("PropertyAdd", property);
        }

        [HttpPost]
        public ActionResult PropertyEdit(MobileProperty model)
        {
            var config = dbContextService.Single<MobileProperty>(model.Id);

            config.Name = model.Name;
            config.BrandId = model.BrandId;
            config.HardwareId = model.HardwareId;
            config.ResoulutionId = model.ResoulutionId;
            config.Status = model.Status;
            config.Comment = model.Comment;
            config.CreateDateTime = DateTime.Now;

            dbContextService.Update<MobileProperty>(config);
            return RedirectToAction("PropertyList");
        }

        public ActionResult PropertyDelete(int propertyId)
        {
            var ret = dbContextService.Delete<MobileProperty>(propertyId);
            return RedirectToAction("PropertyList");
        }

        #endregion

        #region Brand

        public ActionResult BrandList()
        {
            var brandlist = dbContextService.All<MobileBrand>().ToList();
            ViewData["Brandlist"] = brandlist;
            ViewData["TotalCount"] = brandlist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult BrandAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BrandAdd(MobileBrand model)
        {
            var exist = dbContextService.Exists<MobileBrand>(x => x.Value == model.Value);
            if (exist)
            {
                TempData["errorMsg"] = "该品牌已经存在！";
                return View();
            }
            var ret = dbContextService.Add<MobileBrand>(model);

            return RedirectToAction("BrandList");
        }

        [HttpGet]
        public ActionResult BrandEdit(int BrandId)
        {
            var Brand = dbContextService.Single<MobileBrand>(BrandId);
            ViewData["IsUpdate"] = true;
            return View("BrandAdd", Brand);
        }

        [HttpPost]
        public ActionResult BrandEdit(MobileBrand model)
        {
            var brand = dbContextService.Single<MobileBrand>(model.Id);

            brand.Name = model.Name;
            brand.Value = brand.Value;
            brand.Status = model.Status;
            brand.Comment = model.Comment;
            brand.CreateDateTime = DateTime.Now;

            dbContextService.Update<MobileBrand>(brand);
            return RedirectToAction("BrandList");
        }

        public ActionResult BrandDelete(int brandId)
        {
            var ret = dbContextService.Delete<MobileBrand>(brandId);
            return RedirectToAction("BrandList");
        }

        #endregion

        #region Hardware

        public ActionResult HardwareList()
        {
            var hardwarelist = dbContextService.All<MobileHardware>().ToList();
            ViewData["Hardwarelist"] = hardwarelist;
            ViewData["TotalCount"] = hardwarelist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult HardwareAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HardwareAdd(MobileHardware model)
        {
            var exist = dbContextService.Exists<MobileHardware>(x => x.Value == model.Value);
            if (exist)
            {
                TempData["errorMsg"] = "该项目已经存在！";
                return View();
            }
            var ret = dbContextService.Add<MobileHardware>(model);

            return RedirectToAction("HardwareList");
        }

        [HttpGet]
        public ActionResult HardwareEdit(int HardwareId)
        {
            var Hardware = dbContextService.Single<MobileHardware>(HardwareId);
            ViewData["IsUpdate"] = true;
            return View("HardwareAdd", Hardware);
        }

        [HttpPost]
        public ActionResult HardwareEdit(MobileHardware model)
        {
            var hardware = dbContextService.Single<MobileHardware>(model.Id);

            hardware.Name = model.Name;
            hardware.Value = hardware.Value;
            hardware.Status = model.Status;
            hardware.Comment = model.Comment;
            hardware.CreateDateTime = DateTime.Now;

            dbContextService.Update<MobileHardware>(hardware);
            return RedirectToAction("HardwareList");
        }

        public ActionResult HardwareDelete(int hardwareId)
        {
            var ret = dbContextService.Delete<MobileHardware>(hardwareId);
            return RedirectToAction("HardwareList");
        }

        #endregion

        #region Resolution

        public ActionResult ResolutionList()
        {
            var resolutionlist = dbContextService.All<MobileResolution>().ToList();
            ViewData["Resolutionlist"] = resolutionlist;
            ViewData["TotalCount"] = resolutionlist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult ResolutionAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResolutionAdd(MobileResolution model)
        {
            var exist = dbContextService.Exists<MobileResolution>(x => x.Value == model.Value);
            if (exist)
            {
                TempData["errorMsg"] = "该品牌已经存在！";
                return View();
            }
            var ret = dbContextService.Add<MobileResolution>(model);
            return RedirectToAction("ResolutionList");
        }

        [HttpGet]
        public ActionResult ResolutionEdit(int ResolutionId)
        {
            var Resolution = dbContextService.Single<MobileResolution>(ResolutionId);
            ViewData["IsUpdate"] = true;
            return View("ResolutionAdd", Resolution);
        }

        [HttpPost]
        public ActionResult ResolutionEdit(MobileResolution model)
        {
            var resolution = dbContextService.Single<MobileResolution>(model.Id);

            resolution.Name = model.Name;
            resolution.Value = resolution.Value;
            resolution.Status = model.Status;
            resolution.Comment = model.Comment;
            resolution.CreateDateTime = DateTime.Now;

            dbContextService.Update<MobileResolution>(resolution);
            return RedirectToAction("ResolutionList");
        }

        public ActionResult ResolutionDelete(int resolutionId)
        {
            var ret = dbContextService.Delete<MobileResolution>(resolutionId);
            return RedirectToAction("ResolutionList");
        }

        #endregion
    }
}
