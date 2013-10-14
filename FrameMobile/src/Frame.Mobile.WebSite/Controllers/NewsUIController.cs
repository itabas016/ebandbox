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
using SubSonic.Schema;
using FrameMobile.Model;
using NCore;
using FrameMobile.Common;
using System.IO;

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

        public const int pageSize = 20;

        public static string IMAGE_URL_PREFIX_BASE = ConfigKeys.TYD_NEWS_IMAGE_FILE_URL.ConfigValue();

        public string IMAGE_HD_URL_PREFIX = string.Format("{0}/{1}", IMAGE_URL_PREFIX_BASE, "720");
        public string IMAGE_NORMAL_URL_PREFIX = string.Format("{0}/{1}", IMAGE_URL_PREFIX_BASE, "480");

        #endregion

        #region Ctor

        public NewsUIController(INewsUIService newsUIService)
        {
            this.NewsUIService = newsUIService;
        }

        #endregion

        #region News

        public ActionResult NewsManage(int? page)
        {
            int pageNum = page.HasValue ? page.Value : 0;
            PagedList<NewsContent> newslist = dbContextService.GetPaged<NewsContent>("PublishTime desc", pageNum, pageSize);
            ViewData["newslist"] = newslist;
            ViewData["pageNum"] = pageNum;
            return View(newslist);
        }

        [HttpGet]
        public ActionResult NewsAdd()
        {
            var categorylist = NewsUIService.GetNewsCategoryList().ToList();
            var subcategorylist = NewsUIService.GetNewsSubCategoryList().ToList();
            var extraapplist = NewsUIService.GetNewsExtraAppList().ToList();

            ViewData["Categorylist"] = categorylist.GetSelectList();
            ViewData["SubCategorylist"] = subcategorylist.GetSelectList();
            ViewData["ExtraApplist"] = extraapplist.GetSelectList();

            return View();
        }

        [HttpPost]
        public ActionResult NewsAdd(NewsContent model)
        {
            var ret = dbContextService.Add<NewsContent>(model);
            return RedirectToAction("NewsManage");
        }

        [HttpGet]
        public ActionResult NewsEdit(int newsId)
        {
            var news = dbContextService.Single<NewsContent>(newsId);

            var categorylist = NewsUIService.GetNewsCategoryList().ToList();
            var subcategorylist = NewsUIService.GetNewsSubCategoryList().ToList();
            var extraapplist = NewsUIService.GetNewsExtraAppList().ToList();

            ViewData["Categorylist"] = categorylist.GetSelectList(news.CategoryId);
            ViewData["SubCategorylist"] = subcategorylist.GetSelectList(news.SubCategoryId);
            ViewData["ExtraApplist"] = extraapplist.GetSelectList(news.ExtraAppId);

            ViewData["IsUpdate"] = true;
            return View("NewsAdd", news);
        }

        [HttpPost]
        public ActionResult NewsEdit(NewsContent model, HttpPostedFileBase imageFile)
        {
            var news = dbContextService.Single<NewsContent>(model.Id);

            news.Title = model.Title;
            news.CategoryId = model.CategoryId;
            news.SubCategoryId = model.SubCategoryId;
            news.ExtraAppId = model.ExtraAppId;
            news.Rating = model.Rating;
            news.WAPURL = model.WAPURL;
            news.Site = model.Site;
            news.Summary = model.Summary;
            news.Content = model.Content;
            news.Status = model.Status;
            news.PublishTime = model.PublishTime;
            news.ModifiedTime = DateTime.Now;

            if (imageFile != null)
            {
                var imageURL = imageFile == null ? string.Empty : GetImageURL(imageFile);
                news.HDURL = imageURL == string.Empty ? string.Empty : imageURL;
                news.NormalURL = imageURL == string.Empty ? string.Empty : imageURL;
            }

            dbContextService.Update<NewsContent>(news);

            return RedirectToAction("NewsManage");
        }

        public ActionResult NewsDelete(int newsId)
        {
            var ret = dbContextService.Delete<NewsContent>(x => x.Id == newsId);
            return RedirectToAction("NewsManage");
        }

        public ActionResult NewsSearchResult(int? page)
        {
            int pageNum = page.HasValue ? page.Value : 1;
            var searchKey = Request.QueryString["textfield"];

            var newsResult = dbContextService.Find<NewsContent>(x => x.Title.Contains(searchKey)) as IQueryable<NewsContent>;

            PagedList<NewsContent> newslist = new PagedList<NewsContent>(newsResult, pageNum, pageSize);

            ViewData["newslist"] = newslist;
            ViewData["pageNum"] = pageNum;
            return View("NewsManage", newslist);
        }

        #endregion

        #region NewsConfig

        public ActionResult ConfigList()
        {
            var configlist = dbContextService.All<NewsConfig>();
            ViewData["configlist"] = configlist.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult ConfigAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfigAdd(NewsConfig model)
        {
            var exist = dbContextService.Exists<NewsConfig>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该配置项已存在！";
                return View();
            }
            var ret = dbContextService.Add<NewsConfig>(model);
            return RedirectToAction("ConfigList");
        }

        public ActionResult ConfigDelete(int configId)
        {
            var ret = dbContextService.Delete<NewsConfig>(configId);
            return RedirectToAction("ConfigList");
        }

        [HttpGet]
        public ActionResult ConfigEdit(int configId)
        {
            var config = dbContextService.Single<NewsConfig>(configId);
            ViewData["IsUpdate"] = true;
            return View("ConfigAdd", config);
        }

        [HttpPost]
        public ActionResult ConfigEdit(NewsConfig model)
        {
            var config = dbContextService.Single<NewsConfig>(model.Id);

            config.Name = model.Name;
            config.NameLowCase = model.NameLowCase;
            config.Status = model.Status;
            config.CreateDateTime = DateTime.Now;

            dbContextService.Update<NewsConfig>(config);

            return RedirectToAction("ConfigList");
        }

        #endregion

        #region NewsSource

        public ActionResult SourceList()
        {
            var sourcelist = dbContextService.All<NewsSource>().ToList();
            ViewData["sourcelist"] = sourcelist;
            ViewData["TotalCount"] = sourcelist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult SourceAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SourceAdd(NewsSource model)
        {
            var exist = dbContextService.Exists<NewsSource>(x => x.Name == model.NameLowCase || x.PackageName == model.PackageName);
            if (exist)
            {
                TempData["errorMsg"] = "该提供商已存在！";
                return View();
            }
            var ret = dbContextService.Add<NewsSource>(model);

            NewsUIService.UpdateServerVersion<NewsSource>();

            return RedirectToAction("SourceList");
        }

        public ActionResult SourceDelete(int sourceId)
        {
            var ret = dbContextService.Delete<NewsSource>(sourceId);

            NewsUIService.UpdateServerVersion<NewsSource>();

            return RedirectToAction("SourceList");
        }

        [HttpGet]
        public ActionResult SourceEdit(int sourceId)
        {
            var source = dbContextService.Single<NewsSource>(sourceId);
            ViewData["IsUpdate"] = true;
            return View("SourceAdd", source);
        }

        [HttpPost]
        public ActionResult SourceEdit(NewsSource model)
        {
            var source = dbContextService.Single<NewsSource>(model.Id);

            source.Name = model.Name;
            source.NameLowCase = source.NameLowCase;
            source.PackageName = source.PackageName;
            source.Status = model.Status;
            source.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<NewsSource>(source);

            NewsUIService.UpdateServerVersion<NewsSource>();

            return RedirectToAction("SourceList");
        }

        #endregion

        #region NewsCategory

        public ActionResult CategoryList()
        {
            var categorylist = dbContextService.All<NewsCategory>().ToList();
            ViewData["categorylist"] = categorylist;
            ViewData["TotalCount"] = categorylist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult CategoryAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryAdd(NewsCategory model)
        {
            var exist = dbContextService.Exists<NewsCategory>(x => x.Name == model.Name);
            if (exist)
            {
                TempData["errorMsg"] = "该分类已存在！";
                return View();
            }
            var ret = dbContextService.Add<NewsCategory>(model);

            NewsUIService.UpdateServerVersion<NewsCategory>();

            return RedirectToAction("CategoryList");
        }

        public ActionResult CategoryDelete(int categoryId)
        {
            var ret = dbContextService.Delete<NewsCategory>(categoryId);

            NewsUIService.UpdateServerVersion<NewsCategory>();

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public ActionResult CategoryEdit(int categoryId)
        {
            var category = dbContextService.Single<NewsCategory>(categoryId);
            ViewData["IsUpdate"] = true;
            return View("CategoryAdd", category);
        }

        [HttpPost]
        public ActionResult CategoryEdit(NewsCategory model)
        {
            var category = dbContextService.Single<NewsCategory>(model.Id);

            category.Name = model.Name;
            category.Status = model.Status;
            category.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<NewsCategory>(category);

            NewsUIService.UpdateServerVersion<NewsCategory>();

            return RedirectToAction("CategoryList");
        }

        #endregion

        #region NewsSubCategory

        public ActionResult SubCategoryList()
        {
            var subcategorylist = dbContextService.All<NewsSubCategory>().ToList();
            ViewData["subcategorylist"] = subcategorylist;
            ViewData["TotalCount"] = subcategorylist.Count;

            return View();
        }

        [HttpGet]
        public ActionResult SubCategoryAdd()
        {
            var sourcelist = NewsUIService.GetNewsSourceList().ToList();
            var categorylist = NewsUIService.GetNewsCategoryList().ToList();

            ViewData["Sourcelist"] = sourcelist.GetSelectList();
            ViewData["Categorylist"] = categorylist.GetSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult SubCategoryAdd(NewsSubCategory model)
        {
            var exist = dbContextService.Exists<NewsSubCategory>(x => x.NameLowCase == model.NameLowCase);
            if (exist)
            {
                TempData["errorMsg"] = "该子分类已存在！";
                return View();
            }
            var ret = dbContextService.Add<NewsSubCategory>(model);

            NewsUIService.UpdateServerVersion<NewsSubCategory>();

            return RedirectToAction("SubCategoryList");
        }

        public ActionResult SubCategoryDelete(int subcategoryId)
        {
            var ret = dbContextService.Delete<NewsSubCategory>(subcategoryId);

            NewsUIService.UpdateServerVersion<NewsSubCategory>();

            return RedirectToAction("SubCategoryList");
        }

        [HttpGet]
        public ActionResult SubCategoryEdit(int subcategoryId)
        {
            var subcategory = dbContextService.Single<NewsSubCategory>(subcategoryId);

            var sourcelist = NewsUIService.GetNewsSourceList().ToList();
            var categorylist = NewsUIService.GetNewsCategoryList().ToList();

            ViewData["Sourcelist"] = sourcelist.GetSelectList(subcategory.SourceId);
            ViewData["Categorylist"] = categorylist.GetSelectList(subcategory.CategoryId);

            ViewData["IsUpdate"] = true;
            return View("SubCategoryAdd", subcategory);
        }

        [HttpPost]
        public ActionResult SubCategoryEdit(NewsSubCategory model)
        {
            var subcategory = dbContextService.Single<NewsSubCategory>(model.Id);

            subcategory.Name = model.Name;
            subcategory.NameLowCase = subcategory.NameLowCase;

            subcategory.SourceId = subcategory.SourceId;
            subcategory.CategoryId = subcategory.CategoryId;
            subcategory.Cursor = subcategory.Cursor;
            subcategory.Status = model.Status;
            subcategory.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<NewsSubCategory>(subcategory);

            NewsUIService.UpdateServerVersion<NewsSubCategory>();

            return RedirectToAction("SubCategoryList");
        }

        #endregion

        #region NewsExtraApp

        public ActionResult ExtraAppList()
        {
            var extraapplist = dbContextService.All<NewsExtraApp>().ToList();
            ViewData["extraapplist"] = extraapplist;
            ViewData["TotalCount"] = extraapplist.Count;
            return View();
        }

        [HttpGet]
        public ActionResult ExtraAppAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExtraAppAdd(NewsExtraApp model)
        {
            var exist = dbContextService.Exists<NewsExtraApp>(x => x.Name == model.NameLowCase || x.PackageName == model.PackageName);
            if (exist)
            {
                TempData["errorMsg"] = "该外推应用已存在！";
                return View();
            }
            var ret = dbContextService.Add<NewsExtraApp>(model);

            NewsUIService.UpdateServerVersion<NewsExtraApp>();

            return RedirectToAction("ExtraAppList");
        }

        public ActionResult ExtraAppDelete(int extraAppId)
        {
            var ret = dbContextService.Delete<NewsExtraApp>(extraAppId);

            NewsUIService.UpdateServerVersion<NewsExtraApp>();

            return RedirectToAction("ExtraAppList");
        }

        [HttpGet]
        public ActionResult ExtraAppEdit(int extraAppId)
        {
            var extraApp = dbContextService.Single<NewsExtraApp>(extraAppId);
            ViewData["IsUpdate"] = true;
            return View("ExtraAppAdd", extraApp);
        }

        [HttpPost]
        public ActionResult ExtraAppEdit(NewsExtraApp model)
        {
            var extraApp = dbContextService.Single<NewsExtraApp>(model.Id);

            extraApp.Name = model.Name;
            extraApp.NameLowCase = model.NameLowCase;
            extraApp.PackageName = model.PackageName;
            extraApp.IsBrower = model.IsBrower;
            extraApp.DownloadURL = model.DownloadURL;
            extraApp.Status = model.Status;
            extraApp.CreateDateTime = DateTime.Now;

            var ret = dbContextService.Update<NewsExtraApp>(extraApp);

            NewsUIService.UpdateServerVersion<NewsExtraApp>();

            return RedirectToAction("ExtraAppList");
        }

        #endregion

        #region Helper

        private string GetImageURL(HttpPostedFileBase imageFile)
        {
            var outputURL = SaveResourceFile("D:\\temp", imageFile, string.Format("{1}_{2}", Guid.NewGuid().ToString(), Path.GetExtension(imageFile.FileName)));
            return outputURL;
        }

        protected string SaveResourceFile(string dirPath, HttpPostedFileBase file, string fileName)
        {
            fileName = string.IsNullOrEmpty(fileName) ? file.FileName : fileName;
            var filePath = GetResourceFilePath(dirPath, fileName);

            file.SaveAs(filePath);
            return filePath;
        }

        protected string GetResourceFilePath(string dirPath, string fileName)
        {
            var filePath = string.Empty;
            filePath = Path.Combine(dirPath, fileName);

            return filePath;
        }

        #endregion
    }
}
