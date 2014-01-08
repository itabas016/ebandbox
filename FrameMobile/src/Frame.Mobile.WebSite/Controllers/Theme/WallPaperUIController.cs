using FrameMobile.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Model.Theme;
using FrameMobile.Domain;

namespace Frame.Mobile.WebSite.Controllers
{
    public class WallPaperUIController : ThemeBaseController
    {
        protected override bool IsMobileInterface { get { return false; } }

        #region Category

        public ActionResult CategoryList()
        {
            var categorylist = dbContextService.All<WallPaperCategory>().ToList();
            ViewData["Categorylist"] = categorylist;
            ViewData["TotalCount"] = categorylist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult CategoryAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryAdd(WallPaperCategory model)
        {
            var exist = dbContextService.Exists<WallPaperCategory>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已经存在！";
                return View();
            }
            var ret = dbContextService.Add<WallPaperCategory>(model);

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public ActionResult CategoryEdit(int categoryId)
        {
            var category = dbContextService.Single<WallPaperCategory>(categoryId);
            ViewData["IsUpdate"] = true;
            return View("CategoryAdd", category);
        }

        [HttpPost]
        public ActionResult CategoryEdit(WallPaperCategory model, HttpPostedFileBase logoFile)
        {
            var category = dbContextService.Single<WallPaperCategory>(model.Id);

            category.Name = model.Name;
            category.CategoryLogoUrl = category.CategoryLogoUrl;
            category.Status = model.Status;
            category.OrderNumber = model.OrderNumber;
            category.Comment = model.Comment;
            category.CreateDateTime = DateTime.Now;

            dbContextService.Update<WallPaperCategory>(category);
            return RedirectToAction("CategoryList");
        }

        public ActionResult CategoryDelete(int categoryId)
        {
            var ret = dbContextService.Delete<WallPaperCategory>(categoryId);
            return RedirectToAction("CategoryList");
        }

        #endregion

        #region SubbCategory

        public ActionResult SubCategoryList()
        {
            var subcategorylist = dbContextService.All<WallPaperSubCategory>().ToList();
            ViewData["SubCategorylist"] = subcategorylist;
            ViewData["TotalCount"] = subcategorylist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult SubCategoryAdd()
        {
            var categorylist = WallPaperUIService.GetWallPaperCategoryList();
            ViewData["Categorylist"] = categorylist.GetSelectList();

            return View();
        }

        [HttpPost]
        public ActionResult SubCategoryAdd(WallPaperSubCategory model)
        {
            var exist = dbContextService.Exists<WallPaperSubCategory>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已经存在！";
                return View();
            }
            var ret = dbContextService.Add<WallPaperSubCategory>(model);

            return RedirectToAction("SubCategoryList");
        }

        [HttpGet]
        public ActionResult SubCategoryEdit(int subcategoryId)
        {
            var categorylist = WallPaperUIService.GetWallPaperCategoryList();
            ViewData["Categorylist"] = categorylist.GetSelectList();

            var subcategory = dbContextService.Single<WallPaperSubCategory>(subcategoryId);
            ViewData["IsUpdate"] = true;
            return View("SubCategoryAdd", subcategory);
        }

        [HttpPost]
        public ActionResult SubCategoryEdit(WallPaperSubCategory model, HttpPostedFileBase logoFile)
        {
            var subcategory = dbContextService.Single<WallPaperSubCategory>(model.Id);

            subcategory.Name = model.Name;
            subcategory.SubCategoryLogoUrl = subcategory.SubCategoryLogoUrl;
            subcategory.Status = model.Status;
            subcategory.OrderNumber = model.OrderNumber;
            subcategory.Comment = model.Comment;
            subcategory.CreateDateTime = DateTime.Now;

            dbContextService.Update<WallPaperSubCategory>(subcategory);
            return RedirectToAction("SubCategoryList");
        }

        public ActionResult SubCategoryDelete(int subcategoryId)
        {
            var ret = dbContextService.Delete<WallPaperSubCategory>(subcategoryId);
            return RedirectToAction("SubCategoryList");
        }

        #endregion

        #region Topic

        public ActionResult TopicList()
        {
            var topiclist = dbContextService.All<WallPaperTopic>().ToList();
            ViewData["Topiclist"] = topiclist;
            ViewData["TotalCount"] = topiclist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult TopicAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TopicAdd(WallPaperTopic model)
        {
            var exist = dbContextService.Exists<WallPaperTopic>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已经存在！";
                return View();
            }
            var ret = dbContextService.Add<WallPaperTopic>(model);

            return RedirectToAction("TopicList");
        }

        [HttpGet]
        public ActionResult TopicEdit(int TopicId)
        {
            var Topic = dbContextService.Single<WallPaperTopic>(TopicId);
            ViewData["IsUpdate"] = true;
            return View("TopicAdd", Topic);
        }

        [HttpPost]
        public ActionResult TopicEdit(WallPaperTopic model, HttpPostedFileBase logoFile)
        {
            var topic = dbContextService.Single<WallPaperTopic>(model.Id);

            topic.Name = model.Name;
            topic.TopicLogoUrl = topic.TopicLogoUrl;
            topic.Status = model.Status;
            topic.OrderNumber = model.OrderNumber;
            topic.Comment = model.Comment;
            topic.CreateDateTime = DateTime.Now;

            dbContextService.Update<WallPaperTopic>(topic);
            return RedirectToAction("TopicList");
        }

        public ActionResult TopicDelete(int topicId)
        {
            var ret = dbContextService.Delete<WallPaperTopic>(topicId);
            return RedirectToAction("TopicList");
        }

        #endregion

        #region WallPaper

        public ActionResult WallPaperList()
        {
            var wallpaperlist = dbContextService.All<WallPaper>().ToList();
            ViewData["WallPaperlist"] = wallpaperlist;
            ViewData["TotalCount"] = wallpaperlist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult WallPaperAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WallPaperAdd(WallPaper model)
        {
            var exist = dbContextService.Exists<WallPaper>(x => x.Titile == model.Titile);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已经存在！";
                return View();
            }
            var ret = dbContextService.Add<WallPaper>(model);

            return RedirectToAction("WallPaperList");
        }

        [HttpGet]
        public ActionResult WallPaperEdit(int wallpaperId)
        {
            var wallpaper = dbContextService.Single<WallPaper>(wallpaperId);
            ViewData["IsUpdate"] = true;
            return View("WallPaperAdd", wallpaper);
        }

        [HttpPost]
        public ActionResult WallPaperEdit(WallPaper model)
        {
            var wallpaper = dbContextService.Single<WallPaper>(model.Id);

            wallpaper.Titile = model.Titile;
            wallpaper.ThumbnailName = wallpaper.ThumbnailName;
            wallpaper.Status = model.Status;
            wallpaper.OriginalName = model.OriginalName;
            wallpaper.PublishTime = model.PublishTime;
            wallpaper.ModifiedTime = DateTime.Now;
            wallpaper.Rating = model.Rating;
            wallpaper.DownloadNumber = model.DownloadNumber;
            wallpaper.OrderNumber = model.OrderNumber;
            wallpaper.Comment = model.Comment;
            wallpaper.CreateDateTime = DateTime.Now;

            dbContextService.Update<WallPaper>(wallpaper);
            return RedirectToAction("WallPaperList");
        }

        public ActionResult WallPaperDelete(int wallpaperId)
        {
            var ret = dbContextService.Delete<WallPaper>(wallpaperId);
            return RedirectToAction("WallPaperList");
        }

        #endregion
    }
}
