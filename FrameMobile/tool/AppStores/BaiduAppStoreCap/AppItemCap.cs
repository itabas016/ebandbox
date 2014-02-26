using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RedisMapper;
using TYD.Mobile.Infrastructure.Domain.Services;
using NCore;
using BaiduAppStoreCap.Model;
using TYD.Mobile.Infrastructure.AppStore.Models;
using System.Collections.Specialized;
using BaiduAppStoreCap.Service;
using System.Net;
using System.Threading;
using TYD.Mobile.Infrastructure.Models.ViewModels.AppStores;
using TYD.Mobile.Core.Helpers;
using Newtonsoft.Json.Linq;
using RestSharp.Contrib;

namespace BaiduAppStoreCap
{
    public class AppItemCap
    {
        #region Prop

        private BoardList _boardlist;
        private Board _board;

        private CategoryList _categorylist;
        private Category _category;

        private Content _content;

        private DataConvertService _service;

        public IFileService FileService { get; set; }
        public IRedisService RedisService { get; set; }
        public IAppStoreUIService AppStoreUIService { get; set; }

        public static string AppStoreResourcesDirRoot = AppConfigKey.AppStoreResources_Dir_Root.ConfigValue();
        public static string ResourceDirRoot = AppConfigKey.Share_AppStoreResources_Dir_Root.ConfigValue();

        public string APK_Folder_Base = string.Format("{0}\\AppFiles", AppStoreResourcesDirRoot);
        public string Screenshots_Folder_Base = string.Format("{0}\\Screenshots", AppStoreResourcesDirRoot);
        public string Logo_Folder_Base = string.Format("{0}\\AppLogos", AppStoreResourcesDirRoot);

        public string LogoDirRoot = Path.Combine(ResourceDirRoot, "AppLogos");
        public string ScreenshotDirRoot = Path.Combine(ResourceDirRoot, "Screenshots");
        public static string APKToolPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\aapt.exe");

        #endregion

        #region Ctor

        public AppItemCap()
        {
            _board = new Board();
            _boardlist = new BoardList();

            _category = new Category();
            _categorylist = new CategoryList();

            _content = new Content();

            _service = new DataConvertService();

            FileService = new FileService();
            RedisService = new RedisService();
            AppStoreUIService = new AppStoreUIService(FileService, RedisService);
        }

        #endregion

        #region Method

        public void AppItemCompleteCap()
        {
            ReformApp reformApp = new ReformApp();

            var categorylist = GetAllCategory();
            var applist = GetAllAppItemByCategory(categorylist);

            foreach (var category in categorylist)
            {
                var appItemlist = GetAllAppItemByCategory(category);

                foreach (var item in appItemlist)
                {
                    BuildAppProject(reformApp, item);
                }
            }

            LogHelper.WriteInfo("新增应用数：" + reformApp.NewAppCount);
            LogHelper.WriteInfo("新增apk版本数：" + reformApp.NewVersionCount);
            LogHelper.WriteInfo("已有重复应用apk数：" + reformApp.DupVersionCount);
        }

        #endregion

        #region Get AppItem

        public List<BaiduAppDetail> GetAllAppItem()
        {
            var categorylist = GetAllCategory();
            var applist = GetAllAppItemByCategory(categorylist);
            var appDetaillist = GetAllAppItemByAppId(applist);

            return appDetaillist;
        }

        public List<BaiduAppDetail> GetAllAppItemByCategory(BaiduCategory category)
        {
            int total = 0;
            var applist = GetApplistByCategoryId(category.Id, out total);
            LogHelper.WriteInfo(string.Format("Category {0} has {1} apps", category.Name, total), ConsoleColor.Yellow);
            var appDetaillist = GetAllAppItemByAppId(applist);

            return appDetaillist;
        }

        public List<BaiduAppDetail> GetAllAppItemByAppId(List<BaiduApp> applist)
        {
            var appDetailList = new List<BaiduAppDetail>();

            if (applist != null && applist.Count > 0)
            {
                foreach (var item in applist)
                {
                    var app = GetAppDetail(item.Id);
                    appDetailList.Add(app);
                }
            }
            return appDetailList;
        }

        public BaiduAppDetail GetAppDetail(long appId)
        {
            _content.AppId = appId;
            var ret = _content.GetData(null);

            var appXml = _service.GetXmlDocument(ret);

            var appResult = _service.Deserialize<BaiduContentResult>(appXml);

            var app = appResult.Result.AppDetail;

            return app;
        }

        #region Get BoardAppItem

        public List<BaiduApp> GetAllAppItemByBoard(List<BaiduBoard> boardlist)
        {
            var result = new List<BaiduApp>();

            foreach (var item in boardlist)
            {
                int total = 0;
                var applist = GetApplistByBoardId(item.Id, out total);

                LogHelper.WriteInfo(string.Format("Board {0} has {1} apps", item.Name, total), ConsoleColor.Yellow);
                result = result.Union(applist).ToList();
            }
            return result;
        }

        public List<BaiduApp> GetApplistByBoardId(int boardId, out int total)
        {
            var applistResult = GetApplistByBoardId(boardId);

            var applist = applistResult.AppList;
            total = applistResult.Total;

            var pageApplist = GetBoardApplistByPaged(applist, applistResult, boardId);
            applist = applist.Union(pageApplist).ToList();

            return applist;
        }

        public List<BaiduApp> GetBoardApplistByPaged(List<BaiduApp> applist, BaiduAppListResult applistResult, int boardId)
        {
            var total = applistResult.Total;
            var page = total / 50;
            if (page >= 1)
            {
                for (int i = 1; i < total / 100 + 1; i++)
                {
                    _board.PageNum = 50 * i;
                    var data = _board.GetData(null);

                    var pageApplistResult = GetApplistByBoardId(boardId);

                    if (pageApplistResult.AppList != null && pageApplistResult.AppList.Count > 0)
                    {
                        applist = applist.Union(pageApplistResult.AppList).ToList();
                    }
                }
            }
            return applist;
        }

        public BaiduAppListResult GetApplistByBoardId(int boardId)
        {
            _board.BoardId = boardId;
            var ret = _board.GetData(null);
            var boardXml = _service.GetXmlDocument(ret);
            var boardResult = _service.Deserialize<BaiduBoardResult>(boardXml);

            return boardResult.Result;

        }

        public List<BaiduBoard> GetAllBoard()
        {
            var ret = _boardlist.GetData(null);

            var boardXml = _service.GetXmlDocument(ret);
            var boardlistResult = _service.Deserialize<BaiduBoardListResult>(boardXml);

            return boardlistResult.BoardList;
        }

        #endregion

        #region Get CategoryAppItem

        public List<BaiduApp> GetAllAppItemByCategory(List<BaiduCategory> categorylist)
        {
            var result = new List<BaiduApp>();

            foreach (var item in categorylist)
            {
                int total = 0;
                var applist = GetApplistByCategoryId(item.Id, out total);

                LogHelper.WriteInfo(string.Format("Category {0} has {1} apps", item.Name, total), ConsoleColor.Yellow);
                result = result.Union(applist).ToList();
            }
            return result;

        }

        public List<BaiduApp> GetApplistByCategoryId(int categoryId, out int total)
        {
            var applistResult = GetApplistByCategoryId(categoryId);

            var applist = applistResult.AppList;
            total = applistResult.Total;

            var pageApplist = GetCategoryApplistByPaged(applist, applistResult, categoryId);
            applist = applist.Union(pageApplist).ToList();

            return applist;
        }

        public List<BaiduApp> GetCategoryApplistByPaged(List<BaiduApp> applist, BaiduAppListResult applistResult, int categoryId)
        {
            var total = applistResult.Total;
            var page = total / 50;
            if (page >= 1)
            {
                for (int i = 1; i < total / 100 + 1; i++)
                {
                    _category.PageNum = 50 * i;

                    var pageApplistResult = GetApplistByCategoryId(categoryId);

                    if (pageApplistResult.AppList != null && pageApplistResult.AppList.Count > 0)
                    {
                        applist = applist.Union(pageApplistResult.AppList).ToList();
                    }
                }
            }
            return applist;
        }

        public BaiduAppListResult GetApplistByCategoryId(int categoryId)
        {
            _category.CategoryId = categoryId;
            var ret = _category.GetData(null);
            var categoryXml = _service.GetXmlDocument(ret);
            var categoryResult = _service.Deserialize<BaiduCategoryResult>(categoryXml);

            return categoryResult.Result;
        }

        public List<BaiduCategory> GetAllCategory()
        {
            var ret = _categorylist.GetData(null);

            var cateXml = _service.GetXmlDocument(ret);
            var categorylistResult = _service.Deserialize<BaiduCategoryListResult>(cateXml);

            return categorylistResult.CategoryList;
        }

        public bool IsPaged(BaiduAppListResult applistResult)
        {
            var total = applistResult.Total;
            var pagenum = applistResult.PageNum;
            var count = applistResult.Count;
            var flag = total > pagenum + count ? true : false;
            return flag;
        }

        #endregion

        #endregion

        #region Build

        public void BuildAppProject(ReformApp reformApp, BaiduAppDetail appItem)
        {
            var appProject = AppStoreUIService.GetAppProjectByPKGName(appItem.PackageName);

            if (appProject == null)
            {
                LogHelper.WriteInfo(string.Format("Has new app, name {0}, downloading...", appItem.Name), ConsoleColor.Yellow);
                reformApp.NewAppCount++;

                var appfileName = string.Empty;
                DownloadResources(appItem, out appfileName);

                if (!string.IsNullOrEmpty(appfileName))
                {
                    appProject = AddNewApp(appItem, appProject, appfileName);
                }
            }
            else
            {
                try
                {
                    var appitems = AppStoreUIService.GetAppsFromAppList<AppProject>(appProject.Id);
                    if (appitems == null)
                    {
                        AppProjectDelete(appProject.Id);
                    }
                    else
                    {
                        AddNewVersionApp(reformApp, appItem, appProject);
                    }
                }
                catch (Exception)
                {
                    AppProjectDelete(appProject.Id);
                }
            }
        }

        public AppProject AddNewApp(BaiduAppDetail appItem, AppProject appProject, string appfileName)
        {
            try
            {
                var app = new App();

                appProject = SetupAppList(appProject, out app);
                appProject = SetupAppProject(appItem, appProject);
                app = SetupApp(appItem, appProject, app);
                SetupAppVersion(appItem, app, appfileName);
                SetupTags(appItem, appProject, app);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
                AppProjectDelete(appProject.Id);
                LogHelper.WriteInfo(string.Format("AppProjectId: {0} is delete.", appProject.Id));
            }
            return appProject;
        }

        public void AddNewVersionApp(ReformApp reformApp, BaiduAppDetail appItem, AppProject appProject)
        {
            try
            {
                var appitems = AppStoreUIService.GetAppsFromAppList<AppProject>(appProject.Id);
                foreach (var a in appitems)
                {
                    var versions = RedisService.GetAllSubModelIdsByType<App, AppVersion>(a.Id).ToIdsWithNoPrefix<AppVersion>();

                    if (!versions.Contains(appItem.VersionCode))
                    {
                        reformApp.NewVersionCount = AddNewVersionForApp(reformApp.NewVersionCount, appItem, a);
                    }
                    else
                    {
                        reformApp.DupVersionCount++;
                        LogHelper.WriteInfo(string.Format("Already has version {1} for app name {0}", appItem.Name, appItem.VersionCode), ConsoleColor.DarkYellow);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
            }
        }

        private int AddNewVersionForApp(int newVersionCount, BaiduAppDetail appItem, App app)
        {
            newVersionCount++;

            var isTYD = CheckTYDApp(appItem, app);
            var isTencent = CheckTencentApp(appItem, app);

            if (!isTYD && !isTencent)
            {
                var appfileName = string.Empty;
                DownloadResources(appItem, out appfileName);

                if (!string.IsNullOrEmpty(appfileName))
                {
                    SetupAppVersion(appItem, app, appfileName);

                    SetupTags(appItem, app);
                }
            }
            return newVersionCount;
        }

        #endregion

        #region Download

        public void DownloadResources(BaiduAppDetail appItem, out string appfileName)
        {
            appfileName = string.Empty;
            if (appItem != null)
            {
                MakeSureDIRExist(APK_Folder_Base);
                MakeSureDIRExist(Screenshots_Folder_Base);
                MakeSureDIRExist(Logo_Folder_Base);

                var iconFileName = GetFileNameFromUri(GetDownloadUrl(appItem.IconUrl));
                var iconFilePath = Path.Combine(Logo_Folder_Base, iconFileName);
                DownloadFile(appItem.IconUrl, iconFilePath);

                var screenshotlist = GetScreenShotlist(appItem);
                foreach (var img in screenshotlist)
                {
                    var screenshotFileName = GetFileNameFromUri(GetDownloadUrl(img));
                    var screenshotFilePath = Path.Combine(Screenshots_Folder_Base, screenshotFileName);
                    DownloadFile(img, screenshotFilePath);
                }

                var appdownloadurl = GetRedirectUrl(appItem.DownloadUrlDetail, out appfileName);
                var apkFilePath = Path.Combine(APK_Folder_Base, appfileName.MakeSureNotNull());
                DownloadFile(appdownloadurl, apkFilePath);
            }
        }

        #endregion

        #region Setup

        public AppProject SetupAppList(AppProject appProject, out App app)
        {
            appProject = new AppProject();
            var appProjectId = RedisService.Add<AppProject>(appProject);

            app = new App();
            var appId = RedisService.Add<App>(app);

            AppSettingsForAppList appSetting = new AppSettingsForAppList()
            {
                Id = appId,
                CreateDateTime = DateTime.Now
            };

            CustomProperty prop = new CustomProperty()
            {
                Id = AppConfigKey.OS_ATTR_ID,
                Value = AppConfigKey.OS_ATTR_VALUE
            };

            RedisService.AddCustomPropertyFor<App, CustomProperty>(app.Id, prop);

            var lcdDetails = AppStoreUIService.GetElementDetailList(AppConfigKey.LCD_ATTR_ID);
            foreach (var lcd in lcdDetails)
            {
                SetLCD(app.Id, lcd.Value.ToString());
            }

            AppStoreUIService.SetAppForAppList<AppProject>(appProjectId, appSetting);

            return appProject;
        }

        public AppProject SetupAppProject(BaiduAppDetail appItem, AppProject appProject)
        {
            var originalAppProject = CloneHelper.DeepClone<AppProject>(appProject);

            appProject.AppNo = "baidu_" + appItem.Id;
            appProject.Creator = appItem.SourceName;
            appProject.LogoFile = GetFileNameFromUri(GetDownloadUrl(appItem.IconUrl));
            appProject.Name = appItem.Name;
            appProject.PackageName = appItem.PackageName;
            //appProject.Rate = appItem.Rating.ToInt32();
            RedisService.UpdateWithRebuildIndex<AppProject>(originalAppProject, appProject);

            return appProject;
        }

        public App SetupApp(BaiduAppDetail appItem, AppProject appProject, App app)
        {
            var originalApp = CloneHelper.DeepClone<App>(app);
            var originalApp2 = RedisService.Get<App>(app.Id);

            app.AppNo = appProject.AppNo;
            app.AppProjectId = appProject.Id;
            app.UseGreaterVersion = true;
            ClientImageInfo lg = new ClientImageInfo
            {
                BelongsToAppId = app.Id,
                FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(GetDownloadUrl(appItem.IconUrl))),
                TypeId = AppConfigKey.CLIENT_IMAGE_TYPE_ID
            };
            RedisService.Add<ClientImageInfo>(lg);
            app.ClientLogos = new List<ClientImageInfo> 
                            {
                                lg
                            };

            ImageInfo lg2 = new ImageInfo
            {
                BelongsToAppId = app.Id,
                FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(GetDownloadUrl(appItem.IconUrl)))
            };
            RedisService.Add<ImageInfo>(lg2);
            app.Logo = lg2;

            app.Name = appItem.Name;
            app.OrderNumber = appItem.Score;
            app.DownloadTimes = appItem.Score;
            app.Status = 1;
            var screenShotlist = GetScreenShotlist(appItem);
            foreach (var s in screenShotlist)
            {
                ImageInfo ss = new ImageInfo
                {
                    BelongsToAppId = app.Id,
                    FileUrl = Path.Combine(ScreenshotDirRoot, GetFileNameFromUri(GetDownloadUrl(s)))
                };
                RedisService.Add<ImageInfo>(ss);
                app.ScreenShot.Add(ss);
            }
            app.PlatformType = AppConfigKey.PLATFORM_TYPE_ID.ConfigValue().ToInt32();
            app.Summary = appItem.Summary.Replace("<br/>", string.Empty).Replace("<br>", string.Empty);
            var changeLog = appItem.ChangeLog.Replace("<br/>", string.Empty).Replace("<br>", string.Empty).Replace("NULL", string.Empty);
            app.Summary = string.Format("{0}\r\n{1}", app.Summary, changeLog);
            RedisService.UpdateWithRebuildIndex<App>(originalApp2, app);

            return app;
        }

        public void SetupAppVersion(BaiduAppDetail appItem, App app, string appfileName)
        {
            if (!string.IsNullOrEmpty(appItem.DownloadUrlDetail))
            {
                FileInfo fi = new FileInfo(Path.Combine(APK_Folder_Base, appfileName));

                if (fi != null && fi.Exists)
                {
                    AppVersion ver = new AppVersion
                    {
                        FileSize = (int)fi.Length,
                        FileUrl = appfileName,
                        PublishDateTime = appItem.UpdateTimeDetail.ToExactDateTime("yyyy-MM-dd"),
                        Status = 1,
                        VersionName = appItem.VersionName,
                        Id = appItem.VersionCode
                    };

                    AndroidPackageView apkInfo = FileService.GetAndroidPackageInfomation(fi.FullName);
                    apkInfo.Id = ver.Id;

                    var originalApp = RedisService.Get<App>(app.Id);
                    if (app.Status == 0)
                    {
                        RedisService.UpdateWithRebuildIndex<App>(originalApp, app);
                        LogHelper.WriteInfo(string.Format("This App {0} status is invaild", app.Name), ConsoleColor.Gray);
                    }

                    RedisService.SetSubModel<App, AppVersion>(app.Id, ver);
                    RedisService.SetSubModel<App, AndroidPackageView>(app.Id, apkInfo);
                    AppStoreUIService.SetAppCurrentTestVersion(app.Id, ver.Id);
                    AppStoreUIService.PublishAppVersion(app.Id);
                }
            }
        }

        public void SetupTags(BaiduAppDetail appItem, AppProject appProject, App app)
        {
            if (!string.IsNullOrEmpty(appItem.Type))
            {
                switch (appItem.Type)
                {
                    case "soft":
                        AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_SOFTWARE, appProject.Id);
                        AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_TOT_10_SOFTWARE, appProject.Id);
                        break;
                    case "game":
                        AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_GAME, appProject.Id);
                        AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_TOT_10_GAMES, appProject.Id);
                        break;
                    default:
                        break;
                }

                AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_LATEST, appProject.Id);
                AppStoreUIService.AddTagForAppProject(appItem.CategoryName, appProject.Id);

                AppStoreUIService.AddTagForApp(appItem.CategoryName, app.Id);
                AppStoreUIService.AddTagForApp(AppConfigKey.TAG_LIVE, app.Id);
                AppStoreUIService.AddTagForApp(AppConfigKey.TAG_VALID, app.Id);
                AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_FROM_BAIDU, appProject.Id);
            }
        }

        public void SetupTags(BaiduApp appItem, App app)
        {
            if (app.Status != 0)
                AppStoreUIService.AddTagForApp(AppConfigKey.TAG_LIVE, app.Id);
            AppStoreUIService.AddTagForApp(appItem.CategoryName, app.Id);
            AppStoreUIService.AddTagForApp(AppConfigKey.TAG_FROM_BAIDU, app.Id);
            AppStoreUIService.AddTagForApp(AppConfigKey.TAG_LIVE, app.Id);
        }

        #endregion

        #region Delete

        public void AppProjectDelete(string appProjectId)
        {
            DeleteTags(appProjectId);
            DeleteAppsByAppProject(appProjectId);

            RedisService.DeleteWithCustomProperties<AppProject, CustomProperty>(appProjectId);
        }

        public void DeleteAppsByAppProject(string appProjectId)
        {
            var apps = this.AppStoreUIService.GetAppsFromAppList<AppProject>(appProjectId);

            if (apps != null)
            {
                foreach (var app in apps)
                {
                    DeleteTags(app);
                    DeleteRedundanceForAppBranch(app.Id);
                    RedisService.DeleteWithCustomProperties<App, CustomProperty>(app.Id);
                    DeleteAppSettingForAppColumn(app.Id);
                }
            }
        }

        public void DeleteTags(string appProjectId)
        {
            var tags = AppStoreUIService.GetTagsByAppProject(appProjectId);
            if (tags != null)
            {
                foreach (var t in tags)
                {
                    AppStoreUIService.DeleteTagFromAppProject(t.Id, appProjectId);
                }
            }
        }

        public void DeleteTags(App app)
        {
            var appTags = AppStoreUIService.GetTagsByApp(app.Id);
            if (appTags != null)
            {
                foreach (var t in appTags)
                {
                    AppStoreUIService.DeleteTagForApp(t.Id, app.Id);
                }
            }
        }

        private void DeleteRedundanceForAppBranch(string appId)
        {
            //delete LogoImage
            var app = RedisService.Get<App>(appId);
            if (app.Logo != null)
            {
                var logoImage = app.Logo;
                RedisService.Delete<ImageInfo>(logoImage);
            }

            //delete ScreenShotImage
            var screenShotImages = app.ScreenShot;
            foreach (var screenShotImage in screenShotImages)
            {
                RedisService.Delete<ImageInfo>(screenShotImage);
            }

            //delete ClientLogosImage
            var clientLogoImages = app.ClientLogos;
            foreach (var clientLogoImage in clientLogoImages)
            {
                RedisService.Delete<ClientImageInfo>(clientLogoImage);
            }
        }

        private void DeleteAppSettingForAppColumn(string appId)
        {
            var appColumnIds = RedisService.GetAllActiveModelIds<AppColumn>();
            foreach (var appColumnId in appColumnIds)
            {
                var appSettings = RedisService.GetAllSubModelsByType<AppColumn, AppSettingsForAppList>(appColumnId);
                foreach (var appSetting in appSettings)
                {
                    if (appSetting.Id == appId)
                    {
                        AppStoreUIService.DeleteAppFromAppList<AppColumn>(appColumnId, appId);
                    }
                }
            }
        }

        #endregion

        #region Helper

        public void DownloadFile(string fileUrl, string path, bool force = false)
        {
            int retryTimes = 0;
        Label_0002:
            try
            {
                if (File.Exists(path) && (new FileInfo(path)).Length > 0 && !force)
                {
                    LogHelper.WriteInfo("Downloaded already: " + path, ConsoleColor.DarkMagenta);

                    return;
                }
                using (WebClient webClient = new WebClient())
                {
                    Console.WriteLine(fileUrl);
                    webClient.DownloadFile(fileUrl, path);
                }
                LogHelper.WriteInfo("Downloaded file: " + path, ConsoleColor.DarkGreen);
                retryTimes = 0;
            }
            catch (Exception ex)
            {
                Thread.Sleep(500);
                LogHelper.WriteInfo(ex.Message, ConsoleColor.Red);
                retryTimes++;
                if (retryTimes <= 3)
                {
                    goto Label_0002;
                }
            }
        }

        public string GetRedirectUrl(string originalUrl, out string appfileName)
        {
            appfileName = string.Empty;
            var redirectUrl = string.Empty;
            LogHelper.WriteInfo(string.Format("original url : {0}", originalUrl));
            while (true)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(originalUrl);
                request.Referer = originalUrl;
                request.AllowAutoRedirect = false;
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        var status = httpResponse.StatusCode;
                        var location = response.Headers["Location"];
                        var contentdisposition = response.Headers["Content-Disposition"];
                        if (!string.IsNullOrEmpty(location))
                        {
                            LogHelper.WriteInfo(string.Format("rediect url : {0}", location));
                        }
                        if (!string.IsNullOrEmpty(contentdisposition))
                        {
                            redirectUrl = originalUrl;
                            appfileName = contentdisposition.Replace("attachment; filename=", "").Replace("\"", "");
                            break;
                        }
                        if (status == HttpStatusCode.OK && (response.ContentType == "application/vnd.android.package-archive") || response.ContentType.Contains("application/octet-stream") && response.ContentLength > 0)
                        {
                            redirectUrl = originalUrl;
                            appfileName = GetFileNameFromUri(redirectUrl);
                            break;
                        }
                        if (status == HttpStatusCode.Redirect || status == HttpStatusCode.MovedPermanently)
                        {
                            originalUrl = location;
                        }
                        else
                        {
                            LogHelper.WriteInfo(status.ToString());
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteInfo(ex.Message);
                    LogHelper.WriteError(ex.Message);
                    redirectUrl = originalUrl;
                    break;
                    throw;
                }
            }
            return redirectUrl;
        }

        private string GetExtensionType(string contentType)
        {
            var type = string.Empty;
            switch (contentType)
            {
                case "image/jpeg":
                case "image/pjpeg":
                    type = ".jpg";
                    break;
                case "image/gif":
                    type = ".gif";
                    break;
                case "image/png":
                case "image/x-png":
                    type = ".png";
                    break;
                case "image/x-ms-bmp":
                    type = ".bmp";
                    break;
                case "text/plain":
                case "text/richtext":
                case "text/html":
                    type = ".txt";
                    break;
                case "application/zip":
                case "application/x-zip-compressed":
                    type = ".zip";
                    break;
                case "application/x-rar-compressed":
                    type = ".rar";
                    break;
                default:
                    break;
            }
            return type;
        }

        public string GetDownloadUrl(string fullDownloadUrl)
        {
            var collectionKey = AppConfigKey.PARAMETER_DOWNLOADURL;

            Uri uri = new Uri(fullDownloadUrl);

            var queryString = uri.Query;

            NameValueCollection collection = GetQueryString(queryString);

            return collection[collectionKey];
        }

        public NameValueCollection GetQueryString(string queryString)
        {
            return GetQueryString(queryString, Encoding.UTF8);
        }

        public NameValueCollection GetQueryString(string queryString, Encoding encoding)
        {
            queryString = queryString.Replace("?", "");
            NameValueCollection result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    result[UrlDeCode(key, encoding)] = UrlDeCode(value, encoding);
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }

        public string UrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }

        public void Download(string url)
        {
            string redirectUrl;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Referer = url;
            request.AllowAutoRedirect = false;

            using (WebResponse response = request.GetResponse())
            {
                long fileLength = response.ContentLength;
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                byte[] bufferbyte = new byte[fileLength];
                redirectUrl = response.Headers["Location"];

                int MaxPro = (int)bufferbyte.Length;
                int Currentvalue = 0;
                while (fileLength > 0)
                {
                    int downByte = stream.Read(bufferbyte, Currentvalue, MaxPro);
                    if (downByte == 0) { break; };
                    Currentvalue += downByte;
                    MaxPro -= downByte;
                }

                var fileName = string.IsNullOrEmpty(redirectUrl) ? GetFileNameBySplit(response.ResponseUri.AbsoluteUri) : GetFileNameBySplit(redirectUrl);

                FileStream fs = new FileStream(string.Format("@D:\\temp\\{1}", fileName.MakeSureNotNull()), FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(bufferbyte, 0, bufferbyte.Length);
            }
        }

        public string GetFileNameBySplit(string urlPath)
        {
            var fileArray = urlPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var fileName = fileArray[fileArray.Length - 1];
            return fileName;
        }

        public string GetFileNameFromUri(string uriPath)
        {
            Uri uri = new Uri(uriPath);
            return uri.AbsolutePath.Replace("/", "_");
        }

        public void SetLCD(string appId, string lcd)
        {
            var existedLcdSettings = RedisService.GetCustomPropertyFrom<App, CustomProperty>(appId, AppConfigKey.LCD_ATTR_ID);
            if (existedLcdSettings != null)
            {
                var valJSON = existedLcdSettings.Value as JArray;

                List<string> val = null;
                if (valJSON != null)
                {
                    val = new List<string>();
                    foreach (var v in valJSON)
                    {
                        AddUniqueItemToList(val, v.ToString());
                    }
                }

                if (val != null)
                {
                    AddUniqueItemToList(val, lcd);
                }
                else
                {
                    val = new List<string> { lcd };
                }
                existedLcdSettings.Value = val;
                RedisService.AddCustomPropertyFor<App, CustomProperty>(appId, existedLcdSettings);
            }
            else
            {
                CustomProperty cp = new CustomProperty
                {
                    Id = AppConfigKey.LCD_ATTR_ID,
                    Value = new List<string> { lcd }
                };
                RedisService.AddCustomPropertyFor<App, CustomProperty>(appId, cp);
            }

            Console.WriteLine(string.Format("Set lcd [{0}] for App ID [{1}]", lcd, appId));
        }

        public void AddUniqueItemToList(List<string> list, string val)
        {
            if (!list.Contains(val))
            {
                list.Add(val);
            }
        }

        public void AddMarketTag(string category, string appId)
        {
            switch (category)
            {
                #region Soft
                case "地图导航":
                case "生活实用":
                case "理财购物":
                    AppStoreUIService.AddTagForApp("生活地图", appId);
                    break;
                case "系统安全":
                    AppStoreUIService.AddTagForApp("系统输入", appId);
                    break;
                case "聊天通讯":
                    AppStoreUIService.AddTagForApp("聊天通讯", appId);
                    break;
                case "影音图像":
                    AppStoreUIService.AddTagForApp("影音图像", appId);
                    break;
                case "壁纸美化":
                    AppStoreUIService.AddTagForApp("壁纸主题", appId);
                    break;
                case "书籍阅读":
                    AppStoreUIService.AddTagForApp("阅读学习", appId);
                    break;
                case "网络社区":
                    AppStoreUIService.AddTagForApp("网络邮件", appId);
                    break;
                case "学习办公":
                    AppStoreUIService.AddTagForApp("办公商务", appId);
                    break;
                #endregion

                #region Game
                case "休闲益智":
                    AppStoreUIService.AddTagForApp("休闲益智", appId);
                    break;
                case "体育竞速":
                    AppStoreUIService.AddTagForApp("体育竞技", appId);
                    break;
                case "卡片棋牌":
                    AppStoreUIService.AddTagForApp("棋牌天地", appId);
                    break;
                case "动作格斗":
                    AppStoreUIService.AddTagForApp("动作冒险", appId);
                    break;
                case "飞行射击":
                    AppStoreUIService.AddTagForApp("飞行射击", appId);
                    break;
                case "策略游戏":
                case "经营养成":
                    AppStoreUIService.AddTagForApp("经营策略", appId);
                    break;
                case "角色冒险":
                    AppStoreUIService.AddTagForApp("角色扮演", appId);
                    break;
                case "网游":
                    AppStoreUIService.AddTagForApp("网络游戏", appId);
                    break;
                #endregion
            }
        }

        public bool CheckTYDApp(BaiduAppDetail appItem, App app)
        {
            var tags = AppStoreUIService.GetTagsByApp(app.Id);

            if (tags.FindIndex(x => (x.Name == AppConfigKey.TAG_TYD_SKIP) || (x.Id == AppConfigKey.TAG_TYD_SKIP_ID)) != -1)
            {
                LogHelper.WriteInfo(string.Format("TYD手动维护 -- {0}, skipped", appItem.Name), ConsoleColor.Yellow);
                return true;
            }
            LogHelper.WriteInfo(string.Format("Has new version for app, name {0}, downloading...", appItem.Name), ConsoleColor.Yellow);
            return false;
        }

        public bool CheckTencentApp(BaiduAppDetail appItem, App app)
        {
            var tags = AppStoreUIService.GetTagsByApp(app.Id);

            if (app.AppNo.StartsWith("tencent_") || tags.FindIndex(x => (x.Name == AppConfigKey.TAG_FROM_TENCENT)) != -1)
            {
                LogHelper.WriteInfo(string.Format("Tencent App -- {0}, skipped versionupdate", appItem.Name), ConsoleColor.Yellow);
                return true;
            }
            LogHelper.WriteInfo(string.Format("Has new version for app, name {0}, downloading...", appItem.Name), ConsoleColor.Yellow);
            return false;
        }

        public void MakeSureDIRExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public string[] GetScreenShotlist(BaiduAppDetail appItem)
        {
            var imagelist = appItem.ScreenShot.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            return imagelist;
        }

        #endregion
    }
}
