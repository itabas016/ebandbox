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
using FrameMobile.Core;

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

            if (!string.IsNullOrEmpty(logoFilePath))
            {
                model.CategoryLogoUrl = string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));
            }

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
            if (!string.IsNullOrEmpty(logoFilePath))
            {
                category.CategoryLogoUrl = string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));
            }

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
            if (!string.IsNullOrEmpty(logoFilePath))
            {
                model.SubCategoryLogoUrl = string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));
            }

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
            if (!string.IsNullOrEmpty(logoFilePath))
            {
                subcategory.SubCategoryLogoUrl = string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));
            }

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
            if (!string.IsNullOrEmpty(logoFilePath))
            {
                model.TopicLogoUrl = string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));
            }

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
            if (!string.IsNullOrEmpty(logoFilePath))
            {
                topic.TopicLogoUrl = string.Format("{0}{1}", THEME_LOGO_IMAGE_PREFIX, Path.GetFileName(logoFilePath));
            }

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
            var originalFile = Request.Files[Request.Files.Keys[0]];

            var imagePixel = originalFile.GetFilePixel();
            var thumbnailPixel = imagePixel.GetThumbnailPixelByOriginal();
            if (imagePixel == thumbnailPixel)
            {
                TempData["errorMsg"] = "请上传指定像素的图片！";
                return View();
            }
            var thumbnailPixelWidth = thumbnailPixel.GetWidth();
            var thumbnailPixelHeight = thumbnailPixel.GetHeight();

            var originalFilePath = GetThemeOriginalFilePath(model, originalFile);
            if (!string.IsNullOrEmpty(originalFilePath))
            {
                model.OriginalName = string.IsNullOrEmpty(originalFilePath) ? string.Empty : Path.GetFileName(originalFilePath);
            }

            var thumbnailFilePath = ImageHelper.Resized(originalFile, string.Format("{0}{1}\\", GetResourcePathThemeBase(), Const.THEME_THUMBNAILS_FOLDER_NAME), string.Empty, thumbnailPixelWidth, thumbnailPixelHeight);
            if (!string.IsNullOrEmpty(thumbnailFilePath))
            {
                model.ThumbnailName = string.IsNullOrEmpty(thumbnailFilePath) ? string.Empty : Path.GetFileName(thumbnailFilePath);
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

        [AdminAuthorize(UserGroups = "WallPaperAdministrator,WallPaperOperator")]
        [HttpPost]
        public ActionResult WallPaperEdit(WallPaper model, HttpPostedFileBase thumbnailFile, HttpPostedFileBase originalFile)
        {
            var wallpaper = dbContextService.Single<WallPaper>(model.Id);

            wallpaper.Title = model.Title;
            wallpaper.Status = model.Status;
            wallpaper.PublishTime = model.PublishTime;
            wallpaper.ModifiedTime = DateTime.Now;
            wallpaper.Rating = model.Rating;
            wallpaper.DownloadNumber = model.DownloadNumber;
            wallpaper.OrderNumber = model.OrderNumber;
            wallpaper.Comment = model.Comment;
            wallpaper.CreateDateTime = DateTime.Now;

            var thumbnailFilePath = GetThemeThumbnailFilePath(model, thumbnailFile);
            if (!string.IsNullOrEmpty(thumbnailFilePath))
            {
                wallpaper.ThumbnailName = string.IsNullOrEmpty(thumbnailFilePath) ? string.Empty : Path.GetFileName(thumbnailFilePath);
            }

            var originalFilePath = GetThemeOriginalFilePath(model, originalFile);
            if (!string.IsNullOrEmpty(originalFilePath))
            {
                wallpaper.OriginalName = string.IsNullOrEmpty(originalFilePath) ? string.Empty : Path.GetFileName(originalFilePath);
            }

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

            var wallpaperResult = (from p in WallPaperUIService.GetWallPaperList(searchKey)
                                   select new WallPaper
                                   {
                                       Id = p.Id,
                                       Title = p.Title,
                                       DownloadNumber = p.DownloadNumber,
                                       PublishTime = p.PublishTime,
                                       Status = p.Status
                                   }).AsQueryable<WallPaper>();
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
            var resourceFilePath = GetResourcePathThemeBase();
            var wallpaper = dbContextService.Single<WallPaper>(wallpaperId);
            ViewData["WallPaper"] = wallpaper;

            var config = new WallPaperConfigView();
            config.WallPaper = wallpaper;
            var similarRatio = WallPaperUIService.GetImageSimilarRatio(resourceFilePath, wallpaper);

            var originalWidth = WallPaperUIService.GetOriginalImagePixel(resourceFilePath, wallpaper).GetWidth();

            var categorylist = WallPaperUIService.GetWallPaperCategoryList();
            var subcategorylist = WallPaperUIService.GetWallPaperSubCategoryList();
            var topiclist = WallPaperUIService.GetWallPaperTopicList();
            var propertylist = MobileUIService.GetSimilarMobilePropertyList(originalWidth, similarRatio);

            ViewData["Categorylist"] = categorylist.GetSelectList();
            ViewData["SubCategorylist"] = subcategorylist.GetSelectList();
            ViewData["Topiclist"] = topiclist.GetSelectList();
            ViewData["Propertylist"] = propertylist.GetSelectList();

            var relatecategoryIds = WallPaperUIService.GetRelateCategoryIds(wallpaperId).ToList();
            var relatesubcategoryIds = WallPaperUIService.GetRelateSubCategoryIds(wallpaperId).ToList();
            var relatetopicIds = WallPaperUIService.GetRelateTopicIds(wallpaperId).ToList();
            var relatepropertyIds = WallPaperUIService.GetRelateMobilePropertyIds(wallpaperId).ToList();

            var thumbnailNamelist = WallPaperUIService.GetImageNameListByMobileProperty(Const.WALLPAPER_THUMBNAIL, wallpaper, relatepropertyIds).ToList();
            var originalNamelist = WallPaperUIService.GetImageNameListByMobileProperty(Const.WALLPAPER_ORIGINAL, wallpaper, relatepropertyIds).ToList();

            ViewData["ThumbnailNames"] = thumbnailNamelist;
            ViewData["OriginalNames"] = originalNamelist;

            ViewData["RelateCategoryIds"] = relatecategoryIds;
            ViewData["RelateSubCategoryIds"] = relatesubcategoryIds;
            ViewData["RelateTopicIds"] = relatetopicIds;
            ViewData["RelatePropertyIds"] = relatepropertyIds;

            config.RelateCategoryIds = relatecategoryIds;
            config.RelateSubCategoryIds = relatesubcategoryIds;
            config.RelateTopicIds = relatetopicIds;
            config.RelateMobilePropertyIds = relatepropertyIds;
            config.ThumbnailNames = thumbnailNamelist;
            config.OriginalNames = originalNamelist;

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

            var resourceFilePath = GetResourcePathThemeBase();
            WallPaperUIService.WallPaperConfig(model, categoryIds, subcategoryIds, topicIds, propertyIds, resourceFilePath);

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
    }
}
