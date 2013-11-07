using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using QihooAppStoreCap.Invocation;
using QihooAppStoreCap.Model;
using QihooAppStoreCap.Service;
using RedisMapper;
using TYD.Mobile.Core.Helpers;
using TYD.Mobile.Infrastructure.AppStore.Models;
using TYD.Mobile.Infrastructure.Domain.Services;
using NCore;
using TYD.Mobile.Infrastructure.Models.ViewModels.AppStores;
using System.Net;
using System.Threading;
using System.Collections.Specialized;
using RestSharp.Contrib;

namespace QihooAppStoreCap
{
    public class AppItemCap
    {
        #region prop

        private GetApps _app;
        private GetCategorys _category;
        private DataConvertService _service;

        public IFileService FileService { get; set; }
        public IRedisService RedisService { get; set; }
        public IAppStoreUIService AppStoreUIService { get; set; }

        #endregion

        #region Dir Root
        public static string AppStoreResourcesDirRoot = AppConfigKey.AppStoreResources_Dir_Root.ConfigValue();
        public static string ResourceDirRoot = AppConfigKey.Share_AppStoreResources_Dir_Root.ConfigValue();

        public string APK_Folder_Base = string.Format("{0}\\AppFiles", AppStoreResourcesDirRoot);
        public string Screenshots_Folder_Base = string.Format("{0}\\Screenshots", AppStoreResourcesDirRoot);
        public string Logo_Folder_Base = string.Format("{0}\\AppLogos", AppStoreResourcesDirRoot);

        string LogoDirRoot = Path.Combine(ResourceDirRoot, "AppLogos");
        string ScreenshotDirRoot = Path.Combine(ResourceDirRoot, "Screenshots");
        public static string APKToolPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\aapt.exe");
        #endregion

        #region Ctor

        public AppItemCap()
        {
            _app = new GetApps();
            _category = new GetCategorys();
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

        #region Get AppItem

        public List<QihooAppStoreApp> GetAllAppItem(Dictionary<string, string> parameters, out int total)
        {
            var result = new List<QihooAppStoreApp>();
            total = 0;

            var data = _app.GetData(parameters);

            var appResult = _service.DeserializeBase(data);

            if (appResult != null)
            {
                result = appResult.QihooApplist;
                total = appResult.Total;
            }

            return result;
        }

        public List<QihooAppStoreApp> GetAllAppItem(Dictionary<string, string> parameters, int total)
        {
            var result = new List<QihooAppStoreApp>();

            var page = total / 100;
            if (page > 1)
            {
                for (int i = 1; i < total / 100 + 1; i++)
                {
                    parameters["start"] = (i * 100 + 1).ToString();

                    var data = _app.GetData(parameters);

                    var applist = _service.DeserializeAppItem(data);

                    if (applist != null && applist.Count > 0)
                    {
                        result = result.Union(applist).ToList();
                    }
                }
            }
            return result;
        }

        public List<QihooAppStoreApp> GetAllAppItem()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            var startTime = DateTime.Now.AddDays(-1).UnixStamp().ToString();

            parameters["startTime"] = startTime;

            int total = 0;

            var applist = GetAllAppItem(parameters, out total);

            var apppagelist = GetAllAppItem(parameters, total);

            applist = applist.Union(apppagelist).ToList();

            return applist;
        }

        #endregion

        #region Get Category

        public List<QihooAppStoreCategory> GetAllCategory()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            var data = _category.GetData(parameters, true);

            var catlist = _service.DeserializeCategory(data);

            return catlist;

        }

        #endregion

        #region Build

        public void BuildAppProject(ReformApp reformApp, QihooAppStoreApp appItem)
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

        public AppProject AddNewApp(QihooAppStoreApp appItem, AppProject appProject)
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

        public void AddNewVersionApp(ReformApp reformApp, QihooAppStoreApp appItem, AppProject appProject)
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

        private int AddNewVersionForApp(int newVersionCount, QihooAppStoreApp appItem, App app)
        {
            newVersionCount++;

            var flag = CheckTYDApp(appItem, app);

            if (!flag)
            {
                DownloadResources(appItem);

                SetupAppVersion(appItem, app);

                SetupTags(appItem, app);
            }
            return newVersionCount;
        }

        public void AppProjectDelete(string appProjectId)
        {
            DeleteTags(appProjectId);
            DeleteAppsByAppProject(appProjectId);

            RedisService.DeleteWithCustomProperties<AppProject, CustomProperty>(appProjectId);
        }

        #endregion

        #region Download

        public void DownloadResources(QihooAppStoreApp appItem)
        {
            if (appItem != null)
            {
                MakeSureDIRExist(APK_Folder_Base);
                MakeSureDIRExist(Screenshots_Folder_Base);
                MakeSureDIRExist(Logo_Folder_Base);

                DownloadFile(appItem.IconURL, Path.Combine(Logo_Folder_Base, GetFileNameFromUri(appItem.IconURL)));

                var screenshotlist = GetScreenShotlist(appItem);
                foreach (var img in screenshotlist)
                {
                    DownloadFile(img, Path.Combine(Screenshots_Folder_Base, GetFileNameFromUri(img)));
                }
                DownloadFile(appItem.DownloadURL, Path.Combine(APK_Folder_Base, GetFileNameFromUri(GetDownloadUrl(appItem.DownloadURL))));
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

        public AppProject SetupAppProject(QihooAppStoreApp appItem, AppProject appProject)
        {
            var originalAppProject = CloneHelper.DeepClone<AppProject>(appProject);

            appProject.AppNo = "qh360_" + appItem.Id;
            appProject.Creator = appItem.Developer;
            appProject.LogoFile = GetFileNameFromUri(appItem.IconURL);
            appProject.Name = appItem.Name;
            appProject.PackageName = appItem.PackageName;
            appProject.Rate = appItem.Rating;
            RedisService.UpdateWithRebuildIndex<AppProject>(originalAppProject, appProject);

            return appProject;
        }

        public App SetupApp(QihooAppStoreApp appItem, AppProject appProject, App app)
        {
            var originalApp = CloneHelper.DeepClone<App>(app);
            var originalApp2 = RedisService.Get<App>(app.Id);

            app.AppNo = appProject.AppNo;
            app.AppProjectId = appProject.Id;
            app.UseGreaterVersion = true;
            ClientImageInfo lg = new ClientImageInfo
            {
                BelongsToAppId = app.Id,
                FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(appItem.IconURL)),
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
                FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(appItem.IconURL))
            };
            RedisService.Add<ImageInfo>(lg2);
            app.Logo = lg2;

            app.Name = appItem.Name;
            app.OrderNumber = appItem.DownloadTimes.ToInt32();
            app.DownloadTimes = appItem.DownloadTimes.ToInt32();
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
            app.Summary = appItem.Description.Replace("<br/>", string.Empty).Replace("<br>", string.Empty);
            RedisService.UpdateWithRebuildIndex<App>(originalApp2, app);

            return app;
        }

        public void SetupAppVersion(QihooAppStoreApp appItem, App app)
        {
            if (!string.IsNullOrEmpty(appItem.DownloadURL))
            {
                FileInfo fi = new FileInfo(Path.Combine(APK_Folder_Base, GetFileNameFromUri(GetDownloadUrl(appItem.DownloadURL))));

                if (fi != null && fi.Exists)
                {
                    AppVersion ver = new AppVersion
                    {
                        FileSize = (int)fi.Length,
                        FileUrl = GetFileNameFromUri(GetDownloadUrl(appItem.DownloadURL)),
                        PublishDateTime = appItem.UpdateTime.ToExactDateTime("yyyy-MM-dd"),
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

        public void SetupTags(QihooAppStoreApp appItem, AppProject appProject, App app)
        {
            if (appItem.CategoryName.StartsWith(AppConfigKey.CATEGORY_SOFT_NAME, StringComparison.OrdinalIgnoreCase))
            {
                AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_SOFTWARE, appProject.Id);
                AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_TOT_10_SOFTWARE, appProject.Id);
            }
            else
            {
                AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_GAME, appProject.Id);
                AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_TOT_10_GAMES, appProject.Id);
            }
            AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_LATEST, appProject.Id);
            AppStoreUIService.AddTagForAppProject(GetCategoryTagName(appItem.CategoryName), appProject.Id);

            AppStoreUIService.AddTagForApp(GetCategoryTagName(appItem.CategoryName), app.Id);
            AppStoreUIService.AddTagForApp(AppConfigKey.TAG_LIVE, app.Id);
            AppStoreUIService.AddTagForApp(AppConfigKey.TAG_VALID, app.Id);
            AppStoreUIService.AddTagForAppProject(AppConfigKey.TAG_FROM_QIHOO, appProject.Id);
        }

        public void SetupTags(QihooAppStoreApp appItem, App app)
        {
            if (app.Status != 0)
                AppStoreUIService.AddTagForApp(AppConfigKey.TAG_LIVE, app.Id);
            AppStoreUIService.AddTagForApp(GetCategoryTagName(appItem.CategoryName), app.Id);
            AppStoreUIService.AddTagForApp(AppConfigKey.TAG_FROM_QIHOO, app.Id);
            AppStoreUIService.AddTagForApp(AppConfigKey.TAG_LIVE, app.Id);
        }

        #endregion

        #region Delete

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

        #endregion

        #region Helper

        private void DownloadFile(string fileUrl, string path, bool force = false)
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

        private string GetFileNameFromUri(string uriPath)
        {
            Uri uri = new Uri(uriPath);
            return uri.AbsolutePath.Replace("/", "_");
        }

        public string GetDownloadUrl(string fullDownloadUrl)
        {
            Uri uri = new Uri(fullDownloadUrl);

            var queryString = uri.Query;

            NameValueCollection collection = GetQueryString(queryString);

            return collection[AppConfigKey.PARAMETER_DOWNLOADURL];
        }

        public NameValueCollection GetQueryString(string queryString)
        {
            return GetQueryString(queryString, null);
        }

        private NameValueCollection GetQueryString(string queryString, Encoding encoding)
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

        private string UrlDeCode(string str, Encoding encoding)
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

        private void SetLCD(string appId, string lcd)
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

        private void AddUniqueItemToList(List<string> list, string val)
        {
            if (!list.Contains(val))
            {
                list.Add(val);
            }
        }

        private void AddMarketTag(string category, string appId)
        {
            switch (category)
            {
                #region Soft
                case "社交":
                case "生活":
                case "健康":
                case "旅游":
                case "购物":
                case "娱乐":
                    AppStoreUIService.AddTagForApp("生活地图", appId);
                    break;
                case "系统":
                case "安全":
                case "工具":
                    AppStoreUIService.AddTagForApp("系统输入", appId);
                    break;
                case "通讯":
                    AppStoreUIService.AddTagForApp("聊天通讯", appId);
                    break;

                case "音乐":
                case "视频":
                case "摄影":
                    AppStoreUIService.AddTagForApp("影音图像", appId);
                    break;
                case "美化":
                    AppStoreUIService.AddTagForApp("壁纸主题", appId);
                    break;
                case "阅读":
                case "教育":
                    AppStoreUIService.AddTagForApp("阅读学习", appId);
                    break;
                case "新闻":
                    AppStoreUIService.AddTagForApp("网络邮件", appId);
                    break;
                case "办公":
                    AppStoreUIService.AddTagForApp("办公商务", appId);
                    break;
                #endregion

                #region Game
                case "休闲":
                    AppStoreUIService.AddTagForApp("休闲益智", appId);
                    break;
                case "赛车":
                case "体育":
                    AppStoreUIService.AddTagForApp("体育竞技", appId);
                    break;
                case "棋牌":
                    AppStoreUIService.AddTagForApp("棋牌天地", appId);
                    break;
                case "动作":
                case "冒险":
                case "格斗":
                    AppStoreUIService.AddTagForApp("动作冒险", appId);
                    break;
                case "射击":
                    AppStoreUIService.AddTagForApp("飞行射击", appId);
                    break;
                case "战略":
                    AppStoreUIService.AddTagForApp("经营策略", appId);
                    break;
                case "角色":
                    AppStoreUIService.AddTagForApp("角色扮演", appId);
                    break;
                case "网游":
                    AppStoreUIService.AddTagForApp("网络游戏", appId);
                    break;
                #endregion
            }
        }

        private string GetCategoryTagName(string fullCategoryName)
        {
            var prefix = fullCategoryName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var suffix = prefix[0].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            var category = suffix[1].Replace("•", "");

            switch (category)
            {
                case "游戏":
                case "推荐":
                case "软件排行":
                case "游戏排行":
                    category = string.Empty;
                    break;
            }
            return category;
        }

        private bool CheckTYDApp(QihooAppStoreApp appItem, App app)
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

        private void MakeSureDIRExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private string[] GetScreenShotlist(QihooAppStoreApp appItem)
        {
            var imagelist = appItem.ScreenHotsURL.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            return imagelist;
        }

        #endregion
    }
}
