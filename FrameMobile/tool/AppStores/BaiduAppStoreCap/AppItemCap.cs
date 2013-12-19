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

            var appItemlist = GetAllAppItem();

            foreach (var item in appItemlist)
            {
                BuildAppProject(reformApp, item);
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

        public BaiduAppDetail GetAppDetail(int appId)
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

            var pageApplist = GetBoardApplistByPaged(applistResult, boardId);
            applist = applist.Union(pageApplist).ToList();

            return applist;
        }

        public List<BaiduApp> GetBoardApplistByPaged(BaiduAppListResult applistResult, int boardId)
        {
            var applist = new List<BaiduApp>();

            var total = applistResult.Total;
            var page = total / 500;
            if (page >= 1)
            {
                for (int i = 1; i < total / 100 + 1; i++)
                {
                    _board.PageNum = 500 * i;
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

            var pageApplist = GetCategoryApplistByPaged(applistResult, categoryId);
            applist = applist.Union(pageApplist).ToList();

            return applist;
        }

        public List<BaiduApp> GetCategoryApplistByPaged(BaiduAppListResult applistResult, int categoryId)
        {
            var applist = new List<BaiduApp>();

            var total = applistResult.Total;
            var page = total / 500;
            if (page >= 1)
            {
                for (int i = 1; i < total / 100 + 1; i++)
                {
                    _category.PageNum = 500 * i;
                    var data = _category.GetData(null);

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

                DownloadResources(appItem);

                appProject = AddNewApp(appItem, appProject);
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

        public AppProject AddNewApp(BaiduAppDetail appItem, AppProject appProject)
        {
            try
            {
                var app = new App();

                appProject = SetupAppList(appProject, out app);
                appProject = SetupAppProject(appItem, appProject);
                app = SetupApp(appItem, appProject, app);
                SetupTags(appItem, appProject, app);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
                AppProjectDelete(appProject.Id);
                LogHelper.WriteInfo(string.Format("{AppProjectId: {0} is delete.}", appProject.Id));
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
                DownloadResources(appItem);

                SetupAppVersion(appItem, app);

                SetupTags(appItem, app);
            }
            return newVersionCount;
        }

        #endregion

        #region Download

        public void DownloadResources(BaiduApp appItem)
        {
            if (appItem != null)
            {
                MakeSureDIRExist(APK_Folder_Base);
                MakeSureDIRExist(Screenshots_Folder_Base);
                MakeSureDIRExist(Logo_Folder_Base);

                DownloadFile(appItem.IconUrl, Path.Combine(Logo_Folder_Base, GetFileNameFromUri(appItem.IconUrl)));

                var screenshotlist = GetScreenShotlist(appItem);
                foreach (var img in screenshotlist)
                {
                    DownloadFile(img, Path.Combine(Screenshots_Folder_Base, GetFileNameFromUri(img)));
                }
                DownloadFile(appItem.DownloadUrl, Path.Combine(APK_Folder_Base, GetFileNameFromUri(appItem.DownloadUrl)));
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
            appProject.LogoFile = GetFileNameFromUri(appItem.IconUrl);
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
                FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(appItem.IconUrl)),
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
                FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(appItem.IconUrl))
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
                    FileUrl = Path.Combine(ScreenshotDirRoot, GetFileNameFromUri(s))
                };
                RedisService.Add<ImageInfo>(ss);
                app.ScreenShot.Add(ss);
            }
            app.PlatformType = AppConfigKey.PLATFORM_TYPE_ID.ConfigValue().ToInt32();
            app.Summary = appItem.Summary.Replace("<br/>", string.Empty).Replace("<br>", string.Empty);
            var changeLog = appItem.ChangeLog.Replace("<br/>", string.Empty).Replace("<br>", string.Empty);
            app.Summary = string.Format("{0}\r\n{1}", app.Summary, changeLog);
            RedisService.UpdateWithRebuildIndex<App>(originalApp2, app);

            return app;
        }

        public void SetupAppVersion(BaiduAppDetail appItem, App app)
        {
            if (!string.IsNullOrEmpty(appItem.DownloadUrl))
            {
                FileInfo fi = new FileInfo(Path.Combine(APK_Folder_Base, GetFileNameFromUri(appItem.DownloadUrl)));

                if (fi != null && fi.Exists)
                {
                    AppVersion ver = new AppVersion
                    {
                        FileSize = (int)fi.Length,
                        FileUrl = GetFileNameFromUri(appItem.DownloadUrl),
                        PublishDateTime = appItem.UpdateTime,
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

                if (retryTimes > 0)
                {
                    //LogHelper.WriteInfo("Retry to download path: " + fileUrl, ConsoleColor.Magenta);
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

        public bool CheckTYDApp(BaiduApp appItem, App app)
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

        public bool CheckTencentApp(BaiduApp appItem, App app)
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

        public string[] GetScreenShotlist(BaiduApp appItem)
        {
            //var imagelist = appItem.ScreenHotsURL.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return null;
        }

        #endregion
    }
}
