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
using SubSonic.Schema;

namespace Frame.Mobile.WebSite.Controllers
{
    [UserAuthorize(UserGroupTypes = "WallPaper")]
    public class WallPaperUIController : ThemeBaseController
    {
        protected override bool IsMobileInterface { get { return false; } }

        public ActionResult WallPaperManage()
        {
            return RedirectToAction("WallPaperList");
        }

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

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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
            model.CategoryLogoUrl = string.IsNullOrEmpty(logoFilePath) ? string.Empty : string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));

            var ret = dbContextService.Add<WallPaperCategory>(model);
            WallPaperUIService.UpdateServerVersion<WallPaperCategory>();

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public ActionResult CategoryEdit(int categoryId)
        {
            var category = dbContextService.Single<WallPaperCategory>(categoryId);
            ViewData["IsUpdate"] = true;
            return View("CategoryAdd", category);
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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
            category.CategoryLogoUrl = string.IsNullOrEmpty(logoFilePath) ? string.Empty : string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));

            dbContextService.Update<WallPaperCategory>(category);
            WallPaperUIService.UpdateServerVersion<WallPaperCategory>();

            return RedirectToAction("CategoryList");
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        public ActionResult CategoryDelete(int categoryId)
        {
            var ret = dbContextService.Delete<WallPaperCategory>(categoryId);
            WallPaperUIService.UpdateServerVersion<WallPaperCategory>();

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

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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
            model.SubCategoryLogoUrl = string.IsNullOrEmpty(logoFilePath) ? string.Empty : string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));

            var ret = dbContextService.Add<WallPaperSubCategory>(model);
            WallPaperUIService.UpdateServerVersion<WallPaperSubCategory>();

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

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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
            subcategory.SubCategoryLogoUrl = string.IsNullOrEmpty(logoFilePath) ? string.Empty : string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));

            dbContextService.Update<WallPaperSubCategory>(subcategory);
            WallPaperUIService.UpdateServerVersion<WallPaperSubCategory>();

            return RedirectToAction("SubCategoryList");
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        public ActionResult SubCategoryDelete(int subcategoryId)
        {
            var ret = dbContextService.Delete<WallPaperSubCategory>(subcategoryId);
            WallPaperUIService.UpdateServerVersion<WallPaperSubCategory>();

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

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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
            model.TopicLogoUrl = string.IsNullOrEmpty(logoFilePath) ? string.Empty : string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));

            var ret = dbContextService.Add<WallPaperTopic>(model);
            WallPaperUIService.UpdateServerVersion<WallPaperTopic>();

            return RedirectToAction("TopicList");
        }

        [HttpGet]
        public ActionResult TopicEdit(int TopicId)
        {
            var Topic = dbContextService.Single<WallPaperTopic>(TopicId);
            ViewData["IsUpdate"] = true;
            return View("TopicAdd", Topic);
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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
            topic.TopicLogoUrl = string.IsNullOrEmpty(logoFilePath) ? string.Empty : string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));

            dbContextService.Update<WallPaperTopic>(topic);
            WallPaperUIService.UpdateServerVersion<WallPaperTopic>();

            return RedirectToAction("TopicList");
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        public ActionResult TopicDelete(int topicId)
        {
            var ret = dbContextService.Delete<WallPaperTopic>(topicId);
            WallPaperUIService.UpdateServerVersion<WallPaperTopic>();

            return RedirectToAction("TopicList");
        }

        #endregion

        #region WallPaper

        public ActionResult WallPaperList(int? page)
        {
            int pageNum = page.HasValue ? page.Value : 0;
            PagedList<WallPaper> wallpaperlist = dbContextService.GetPaged<WallPaper>("PublishTime desc", pageNum, pageSize);
            ViewData["WallPaperlist"] = wallpaperlist;
            ViewData["pageNum"] = pageNum;
            ViewData["TotalCount"] = wallpaperlist.Count;
            return View(wallpaperlist);
        }

        [HttpGet]
        public ActionResult WallPaperAdd()
        {
            return View();
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        [HttpPost]
        public ActionResult WallPaperAdd(WallPaper model)
        {
            var exist = dbContextService.Exists<WallPaper>(x => x.Title == model.Title);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已经存在！";
                return View();
            }

            var thumbnailFile = Request.Files[Request.Files.Keys[0]];
            var thumbnailFilePath = GetThemeThumbnailFilePath(model, thumbnailFile);
            model.ThumbnailName = string.IsNullOrEmpty(thumbnailFilePath) ? string.Empty : Path.GetFileName(thumbnailFilePath);

            var originalFile = Request.Files[Request.Files.Keys[1]];
            var originalFilePath = GetThemeOriginalFilePath(model, originalFile);
            model.OriginalName = string.IsNullOrEmpty(originalFilePath) ? string.Empty : Path.GetFileName(originalFilePath);

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

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        [HttpPost]
        public ActionResult WallPaperEdit(WallPaper model, HttpPostedFileBase thumbnailFile, HttpPostedFileBase originalFile)
        {
            var wallpaper = dbContextService.Single<WallPaper>(model.Id);

            wallpaper.Title = model.Title;
            wallpaper.WallPaperNo = model.WallPaperNo;
            wallpaper.Status = model.Status;
            wallpaper.PublishTime = model.PublishTime;
            wallpaper.ModifiedTime = DateTime.Now;
            wallpaper.Rating = model.Rating;
            wallpaper.DownloadNumber = model.DownloadNumber;
            wallpaper.OrderNumber = model.OrderNumber;
            wallpaper.Comment = model.Comment;
            wallpaper.CreateDateTime = DateTime.Now;

            var thumbnailFilePath = GetThemeThumbnailFilePath(model, thumbnailFile);
            wallpaper.ThumbnailName = string.IsNullOrEmpty(thumbnailFilePath) ? string.Empty : Path.GetFileName(thumbnailFilePath);

            var originalFilePath = GetThemeOriginalFilePath(model, originalFile);
            wallpaper.OriginalName = string.IsNullOrEmpty(originalFilePath) ? string.Empty : Path.GetFileName(originalFilePath);

            dbContextService.Update<WallPaper>(wallpaper);
            return RedirectToAction("WallPaperList");
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        public ActionResult WallPaperDelete(int wallpaperId)
        {
            var ret = dbContextService.Delete<WallPaper>(wallpaperId);
            return RedirectToAction("WallPaperList");
        }

        public ActionResult WallPaperSearchResult(int? page)
        {
            int pageNum = page.HasValue ? page.Value : 1;
            var searchKey = Request.QueryString["textfield"];

            var wallpaperResult = (from p in dbContextService.All<WallPaper>()
                                   where (p.WallPaperNo == searchKey || p.Title.Contains(searchKey))
                                   select new WallPaper
                                   {
                                       Id = p.Id,
                                       WallPaperNo = p.WallPaperNo,
                                       Title = p.Title,
                                       DownloadNumber = p.DownloadNumber,
                                       PublishTime = p.PublishTime,
                                       Status = p.Status
                                   }).AsQueryable();
            var wallpaperlist = wallpaperResult.ToPagedList<WallPaper>(pageNum, pageSize);

            ViewData["WallPaperlist"] = wallpaperlist;
            ViewData["pageNum"] = pageNum;
            ViewData["TotalCount"] = wallpaperlist.Count;
            return View("WallPaperList", wallpaperlist);
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

            var thumbnailNamelist = WallPaperUIService.GetImageNameListByMobileProperty(Const.WALLPAPER_THUMBNAIL, wallpaper, relatepropertyIds).ToList();
            var originalNamelist = WallPaperUIService.GetImageNameListByMobileProperty(Const.WALLPAPER_ORIGINAL, wallpaper, relatepropertyIds).ToList();

            ViewData["Categorylist"] = categorylist.GetSelectList();
            ViewData["SubCategorylist"] = subcategorylist.GetSelectList();
            ViewData["Topiclist"] = topiclist.GetSelectList();
            ViewData["Propertylist"] = propertylist.GetSelectList();
            ViewData["WallPaper"] = wallpaper;

            ViewData["ThumbnailNames"] = thumbnailNamelist;
            ViewData["OriginalNames"] = originalNamelist;

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
                RelateMobilePropertyIds = relatepropertyIds,
                ThumbnailNames = thumbnailNamelist,
                OriginalNames = originalNamelist
            };

            return View(config);
        }

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
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

        [HttpPost]
        public ActionResult Preview(WallPaperConfigView model, string type)
        {
            var imageViewlist = new List<ImageView>();
            var thumbnailImagePrefix = ConfigKeys.TYD_WALLPAPER_THUMBNAIL_IMAGE_PREFIX.ConfigValue();
            var originalImagePrefix = ConfigKeys.TYD_WALLPAPER_ORIGINAL_IMAGE_PREFIX.ConfigValue();

            switch (type)
            {
                case Const.WALLPAPER_THUMBNAIL:
                    foreach (var item in model.ThumbnailNames)
                    {
                        var imageView = new ImageView();
                        imageView.ImagePath = string.Format("{0}{1}", thumbnailImagePrefix, item);
                        imageViewlist.Add(imageView);
                    }
                    break;
                case Const.WALLPAPER_ORIGINAL:
                    foreach (var item in model.OriginalNames)
                    {
                        var urlView = new ImageView();
                        urlView.ImagePath = string.Format("{0}{1}", originalImagePrefix, item);
                        imageViewlist.Add(urlView);
                    }
                    break;
                default:
                    break;
            }
            ViewData["Imagelist"] = imageViewlist;
            return View();
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
                        var thumbnailFilePath = SaveThemeResourceFile(Const.THEME_THUMBNAILS_FOLDER_NAME, ResourcesFilePathHelper.ThemeThumbnailPath, files[i], string.Format("{0}_{1}{2}", files[i].GetFileNamePrefix(), wallpaper.ThumbnailName.GetFileNamePrefix(), files[i].GetFileType()).NormalzieFileName());
                        continue;
                    }
                    if (files.AllKeys[i].EqualsOrdinalIgnoreCase("originalfile")
                        && !string.IsNullOrWhiteSpace(Request.Files[i].FileName))
                    {
                        var originalFilePath = SaveThemeResourceFile(Const.THEME_ORIGINALS_FOLDER_NAME, ResourcesFilePathHelper.ThemeOriginalPath, files[i], string.Format("{0}_{1}{2}", files[i].GetFileNamePrefix(), wallpaper.OriginalName.GetFileNamePrefix(), files[i].GetFileType()).NormalzieFileName());
                        continue;
                    }
                }
            }
        }

        #endregion
    }
}
