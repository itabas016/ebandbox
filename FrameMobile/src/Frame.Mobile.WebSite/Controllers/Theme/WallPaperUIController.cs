using FrameMobile.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrameMobile.Model.Theme;
using FrameMobile.Domain;
using System.IO;
using FrameMobile.Common;
using NCore;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;

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

            var logoFile = Request.Files[Request.Files.Keys.Count - 1];
            var logoFilePath = GetThemeLogoFilePath<WallPaperCategory>(model, logoFile);
            var logo_Image_Prefix = ConfigKeys.TYD_WALLPAPER_LOGO_IMAGE_PREFIX.ConfigValue();
            model.CategoryLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(logoFilePath));

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
            category.Status = model.Status;
            category.OrderNumber = model.OrderNumber;
            category.Comment = model.Comment;
            category.CreateDateTime = DateTime.Now;

            var logoFilePath = GetThemeLogoFilePath<WallPaperCategory>(model, logoFile);
            var logo_Image_Prefix = ConfigKeys.TYD_WALLPAPER_LOGO_IMAGE_PREFIX.ConfigValue();
            category.CategoryLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(logoFilePath));

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

            var logoFile = Request.Files[Request.Files.Keys.Count - 1];
            var logoFilePath = GetThemeLogoFilePath<WallPaperSubCategory>(model, logoFile);
            var logo_Image_Prefix = ConfigKeys.TYD_WALLPAPER_LOGO_IMAGE_PREFIX.ConfigValue();
            model.SubCategoryLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(logoFilePath));

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
            subcategory.Status = model.Status;
            subcategory.OrderNumber = model.OrderNumber;
            subcategory.Comment = model.Comment;
            subcategory.CreateDateTime = DateTime.Now;

            var logoFilePath = GetThemeLogoFilePath<WallPaperSubCategory>(model, logoFile);
            var logo_Image_Prefix = ConfigKeys.TYD_WALLPAPER_LOGO_IMAGE_PREFIX.ConfigValue();
            subcategory.SubCategoryLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(logoFilePath));

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

            var logoFile = Request.Files[Request.Files.Keys.Count - 1];
            var logoFilePath = GetThemeLogoFilePath<WallPaperTopic>(model, logoFile);
            var logo_Image_Prefix = ConfigKeys.TYD_WALLPAPER_LOGO_IMAGE_PREFIX.ConfigValue();
            model.TopicLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(logoFilePath));

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
            topic.Status = model.Status;
            topic.OrderNumber = model.OrderNumber;
            topic.Summary = model.Summary;
            topic.Comment = model.Comment;
            topic.CreateDateTime = DateTime.Now;

            var logoFilePath = GetThemeLogoFilePath<WallPaperTopic>(model, logoFile);
            var logo_Image_Prefix = ConfigKeys.TYD_WALLPAPER_LOGO_IMAGE_PREFIX.ConfigValue();
            topic.TopicLogoUrl = string.Format("{0}{1}", logo_Image_Prefix, Path.GetFileName(logoFilePath));

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

            var thumbnailFile = Request.Files[Request.Files.Keys[0]];
            var thumbnailFilePath = GetThemeThumbnailFilePath(model, thumbnailFile);
            model.ThumbnailName = Path.GetFileName(thumbnailFilePath);

            var originalFile = Request.Files[Request.Files.Keys[1]];
            var originalFilePath = GetThemeOriginalFilePath(model, originalFile);
            model.OriginalName = Path.GetFileName(originalFilePath);

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
        public ActionResult WallPaperEdit(WallPaper model, HttpPostedFileBase thumbnailFile, HttpPostedFileBase originalFile)
        {
            var wallpaper = dbContextService.Single<WallPaper>(model.Id);

            wallpaper.Titile = model.Titile;
            wallpaper.Status = model.Status;
            wallpaper.PublishTime = model.PublishTime;
            wallpaper.ModifiedTime = DateTime.Now;
            wallpaper.Rating = model.Rating;
            wallpaper.DownloadNumber = model.DownloadNumber;
            wallpaper.OrderNumber = model.OrderNumber;
            wallpaper.Comment = model.Comment;
            wallpaper.CreateDateTime = DateTime.Now;

            var thumbnailFilePath = GetThemeThumbnailFilePath(model, thumbnailFile);
            wallpaper.ThumbnailName = Path.GetFileName(thumbnailFilePath);

            var originalFilePath = GetThemeOriginalFilePath(model, originalFile);
            wallpaper.OriginalName = Path.GetFileName(originalFilePath);

            dbContextService.Update<WallPaper>(wallpaper);
            return RedirectToAction("WallPaperList");
        }

        public ActionResult WallPaperDelete(int wallpaperId)
        {
            var ret = dbContextService.Delete<WallPaper>(wallpaperId);
            return RedirectToAction("WallPaperList");
        }

        #endregion

        #region Config

        [HttpGet]
        public ActionResult WallPaperConfig(int wallpaperId)
        {
            var categorylist = WallPaperUIService.GetWallPaperCategoryList();
            var subcategorylist = WallPaperUIService.GetWallPaperSubCategoryList();
            var topiclist = WallPaperUIService.GetWallPaperTopicList();
            var propertylist = MobileUIService.GetMobilePropertyList();
            var wallpaper = dbContextService.Single<WallPaper>(wallpaperId);

            var relatecategoryIds = WallPaperUIService.GetRelateCategoryIds(wallpaperId).ToList();
            var relatesubcategoryIds = WallPaperUIService.GetRelateSubCategoryIds(wallpaperId).ToList();
            var relatetopicIds = WallPaperUIService.GetRelateTopicIds(wallpaperId).ToList();
            var relatepropertyIds = WallPaperUIService.GetRelateMobilePropertyIds(wallpaperId).ToList();

            ViewData["Categorylist"] = categorylist.GetSelectList();
            ViewData["SubCategorylist"] = subcategorylist.GetSelectList();
            ViewData["Topiclist"] = topiclist.GetSelectList();
            ViewData["Propertylist"] = propertylist.GetSelectList();
            ViewData["WallPaper"] = wallpaper;

            ViewData["RelateCategoryIds"] = relatecategoryIds;
            ViewData["RelateSubCategoryIds"] = relatesubcategoryIds;
            ViewData["RelateTopicIds"] = relatetopicIds;
            ViewData["RelatePropertyIds"] = relatepropertyIds;

            var config = new WallPaperConfigView()
            {
                WallPaper = wallpaper,
                RelateCategoryIds = relatecategoryIds,
                RelateSubCategoryIds = relatesubcategoryIds,
                RelateTopicIds = relatetopicIds,
                RelateMobilePropertyIds = relatepropertyIds
            };

            return View(config);
        }

        [HttpPost]
        public ActionResult WallPaperConfig(WallPaperConfigView model, FormCollection parameters)
        {
            var categoryIds = parameters["category"].GetIds();
            var subcategoryIds = parameters["subcategory"].GetIds();
            var topicIds = parameters["topic"].GetIds();
            var propertyIds = parameters["property"].GetIds();

            WallPaperConfig(model, categoryIds, subcategoryIds, topicIds, propertyIds);

            return RedirectToAction("WallPaperList");
        }

        #endregion

        #region Helper

        #region Connect mysql

        private void WallPaperConfig(WallPaperConfigView model, List<int> categoryIds, List<int> subcategoryIds, List<int> topicIds, List<int> propertyIds)
        {
            var wallpaper = model.WallPaper.MakeSureNotNull() as WallPaper;

            var outcategoryIds = new List<int>();
            var outsubcategoryIds = new List<int>();
            var outtopicIds = new List<int>();
            var outpropertyIds = new List<int>();

            var incategoryIds = InIds(model.RelateCategoryIds, categoryIds, out outcategoryIds);
            var insubcategoryIds = InIds(model.RelateSubCategoryIds, subcategoryIds, out outsubcategoryIds);
            var intopicIds = InIds(model.RelateTopicIds, topicIds, out outtopicIds);
            var inpropertyIds = InIds(model.RelateMobilePropertyIds, propertyIds, out outpropertyIds);

            AddRelateCategory(incategoryIds, wallpaper.Id);
            AddRelateTopic(intopicIds, wallpaper.Id);
            AddRelateMobileProperty(inpropertyIds, wallpaper.Id);

            UpdateRelateCategory(outcategoryIds, wallpaper.Id);
            UpdateRelateSubCategory(outsubcategoryIds, wallpaper.Id);
            UpdateRelateTopic(outtopicIds, wallpaper.Id);
            UpdateRelateMobileProperty(outpropertyIds, wallpaper.Id);

            Upload(wallpaper, inpropertyIds);
        }

        private List<int> InIds(List<int> originalIds, List<int> currentIds, out List<int> outIds)
        {
            var inIds = new List<int>();
            outIds = new List<int>();
            if (currentIds != null && originalIds != null)
            {
                foreach (var currentId in currentIds)
                {
                    if (!originalIds.Contains(currentId))
                    {
                        inIds.Add(currentId);
                    }
                }
                foreach (var originalId in originalIds)
                {
                    if (!currentIds.Contains(originalId))
                    {
                        outIds.Add(originalId);
                    }
                }
            }
            else
            {
                inIds = currentIds;
            }
            return inIds;
        }

        private void AddRelateCategory(List<int> categoryIds, int wallpaperId)
        {
            if (categoryIds != null && categoryIds.Count > 0)
            {
                foreach (var item in categoryIds)
                {
                    var categoryId = item;

                    var flag = UpdateRelateCategory(categoryId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateCategory();
                        model.WallPaperId = wallpaperId;
                        model.CategoryId = categoryId;
                        dbContextService.Add<WallPaperRelateCategory>(model);
                    }
                }
            }
        }

        private void AddRelateSubCategory(List<int> subcategoryIds, int wallpaperId)
        {
            if (subcategoryIds != null && subcategoryIds.Count > 0)
            {
                foreach (var item_sub in subcategoryIds)
                {
                    var subcategoryId = item_sub;

                    var flag = UpdateRelateSubCategory(subcategoryId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateSubCategory();
                        model.WallPaperId = wallpaperId;
                        model.SubCategoryId = subcategoryId;
                        dbContextService.Add<WallPaperRelateSubCategory>(model);
                    }
                }
            }
        }

        private void AddRelateTopic(List<int> topicIds, int wallpaperId)
        {
            if (topicIds != null && topicIds.Count > 0)
            {
                foreach (var item in topicIds)
                {
                    var topicId = item;
                    var flag = UpdateRelateTopic(topicId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateTopic();
                        model.WallPaperId = wallpaperId;
                        model.TopicId = topicId;
                        dbContextService.Add<WallPaperRelateTopic>(model);
                    }
                }
            }
        }

        private void AddRelateMobileProperty(List<int> propertyIds, int wallpaperId)
        {
            if (propertyIds != null && propertyIds.Count > 0)
            {
                foreach (var item in propertyIds)
                {
                    var propertyId = item;
                    var flag = UpdateRelateMobileProperty(propertyId, wallpaperId, 1);
                    if (!flag)
                    {
                        var model = new WallPaperRelateMobileProperty();
                        model.WallPaperId = wallpaperId;
                        model.MobilePropertyId = propertyId;
                        dbContextService.Add<WallPaperRelateMobileProperty>(model);
                    }
                }
            }
        }

        private void UpdateRelateCategory(List<int> categoryIds, int wallpaperId)
        {
            if (categoryIds != null && categoryIds.Count > 0)
            {
                foreach (var item in categoryIds)
                {
                    var categoryId = item;
                    var flag = UpdateRelateCategory(categoryId, wallpaperId, 0);
                }
            }
        }

        private void UpdateRelateSubCategory(List<int> subcategoryIds, int wallpaperId)
        {
            if (subcategoryIds != null && subcategoryIds.Count > 0)
            {
                foreach (var item in subcategoryIds)
                {
                    var subcategoryId = item;
                    var flag = UpdateRelateSubCategory(subcategoryId, wallpaperId, 0);
                }
            }
        }

        private void UpdateRelateTopic(List<int> topicIds, int wallpaperId)
        {
            if (topicIds != null && topicIds.Count > 0)
            {
                foreach (var item in topicIds)
                {
                    var topicId = item;
                    var flag = UpdateRelateTopic(topicId, wallpaperId, 0);
                }
            }
        }

        private void UpdateRelateMobileProperty(List<int> propertyIds, int wallpaperId)
        {
            if (propertyIds != null && propertyIds.Count > 0)
            {
                foreach (var item in propertyIds)
                {
                    var propertyId = item;
                    var flag = UpdateRelateMobileProperty(propertyId, wallpaperId, 0);
                }
            }
        }

        private bool UpdateRelateCategory(int categoryId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateCategory>(x => x.CategoryId == categoryId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateCategory>(instance);
                flag = true;
            }
            return flag;
        }

        private bool UpdateRelateSubCategory(int subcategoryId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateSubCategory>(x => x.SubCategoryId == subcategoryId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateSubCategory>(instance);
                flag = true;
            }
            return flag;
        }

        private bool UpdateRelateTopic(int topicId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateTopic>(x => x.TopicId == topicId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateTopic>(instance);
                flag = true;
            }
            return flag;
        }

        private bool UpdateRelateMobileProperty(int propertyId, int wallpaperId, int status)
        {
            var flag = false;
            var instance = dbContextService.Single<WallPaperRelateMobileProperty>(x => x.MobilePropertyId == propertyId && x.WallPaperId == wallpaperId);
            if (instance != null)
            {
                instance.Status = status;
                dbContextService.Update<WallPaperRelateMobileProperty>(instance);
                flag = true;
            }
            return flag;
        }

        #endregion

        public void Upload(WallPaper wallpaper, List<int> propertyIds)
        {
            var files = Request.Files;
            if (propertyIds != null && propertyIds.Count > 0 && files.Count > 0 && !string.IsNullOrEmpty(wallpaper.ThumbnailName) && !string.IsNullOrEmpty(wallpaper.OriginalName))
            {
                for (int i = 0; i < files.Count; i++)
                {
                    if (files.AllKeys[i].EqualsOrdinalIgnoreCase("thumbnailfile")
                        && !string.IsNullOrWhiteSpace(Request.Files[i].FileName))
                    {
                        var thumbnailFilePath = SaveResourceFile(Const.THEME_THUMBNAILS_FOLDER_NAME, ResourcesFilePathHelper.ThemeThumbnailPath, files[i], string.Format("{0}_{1}{2}", files[i].GetFileNamePrefix(), wallpaper.ThumbnailName.GetFileNamePrefix(), files[i].GetFileType()).NormalzieFileName());
                        continue;
                    }
                    if (files.AllKeys[i].EqualsOrdinalIgnoreCase("originalfile")
                        && !string.IsNullOrWhiteSpace(Request.Files[i].FileName))
                    {
                        var originalFilePath = SaveResourceFile(Const.THEME_ORIGINALS_FOLDER_NAME, ResourcesFilePathHelper.ThemeOriginalPath, files[i], string.Format("{0}_{1}{2}", files[i].GetFileNamePrefix(), wallpaper.OriginalName.GetFileNamePrefix(), files[i].GetFileType()).NormalzieFileName());
                        continue;
                    }
                }
            }
        }

        #endregion
    }
}
