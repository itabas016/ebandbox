using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TencentAppStoreModel;
using Newtonsoft.Json;
using TYD.Mobile.Core;
using System.IO;
using TYD.Mobile.Infrastructure.AppStore.Models;
using TYD.Mobile.Infrastructure.Domain.Services;
using TYD.Mobile.Core.Helpers;
using System.Security.Policy;
using TYD.Mobile.Infrastructure.Models.ViewModels.AppStores;
using Newtonsoft.Json.Linq;
using RestSharp;
using TencentAppStoreModel.TydAppStore;
using TencentAppStoreCap.Connectable;
using RedisMapper;
using NLog;
using TencentAppStoreCap.Const;
using AppListItem = TencentAppStoreModel.AppListItem;
using NCore;

namespace TencentAppStoreCap
{
    public class AppInfoCollector
    {
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
        public IFileService FileService { get; set; }
        public IRedisService RedisService { get; set; }
        public IAppStoreUIService AppStoreUIService { get; set; }

        public AppInfoCollector()
        {
            FileService = new FileService();
            RedisService = new RedisService();
            AppStoreUIService = new AppStoreUIService(FileService, RedisService);
        }

        public AppInfoCollector(IFileService fileService, IRedisService redisService, IAppStoreUIService appStoreUIService)
        {
            this.FileService = fileService;
            this.RedisService = redisService;
            this.AppStoreUIService = appStoreUIService;
        }
        #endregion

        #region Method
        public void PerformFullAppCollect()
        {
            ReformApp reformApp = new ReformApp();

            List<TencentAppStoreModel.AppListItem> apps = GetAllAppInfos();
            foreach (var app in apps)
            {
                BuildAppProjectByNewApp(reformApp, app);
            }

            LogHelper.WriteInfo("新增应用数：" + reformApp.NewAppCount);
            LogHelper.WriteInfo("新增apk版本数：" + reformApp.NewVersionCount);
            LogHelper.WriteInfo("已有重复应用apk数：" + reformApp.DupVersionCount);
        }
        #endregion

        #region Helper

        #region Build AppProject
        private void BuildAppProjectByNewApp(ReformApp reformApp, AppListItem app)
        {
            if (app != null)
            {
                var appProject = AppStoreUIService.GetAppProjectByPKGName(app.packageName);

                if (appProject == null)
                {
                    var isExist = AppStoreUIService.GetAppProjectByAppNo("tencent_" + app.appid);
                    if (isExist == null)
                    {
                        LogHelper.WriteInfo(string.Format("Has new app, name {0}, downloading...", app.name), ConsoleColor.Yellow);
                        reformApp.NewAppCount++;
                        AppInfo appInfo = DowloadAppInfoAndResources(app.appid.ToString());
                        appProject = AddNewApp(app, appProject, appInfo);
                    }
                    else
                    {
                        LogHelper.WriteInfo(string.Format("This app, name {0}, exist same appno, skipped", app.name), ConsoleColor.Yellow);
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
                            AddNewVersionApp(reformApp, app, appProject);
                        }
                    }
                    catch (Exception)
                    {
                        AppProjectDeleteWithEmpty(appProject.Id);
                    }
                }
            }
        }

        private AppProject AddNewApp(TencentAppStoreModel.AppListItem app, AppProject appProject, AppInfo appInfo)
        {
            try
            {
                #region Set up Applist
                appProject = new AppProject();
                var appProjectId = RedisService.Add<AppProject>(appProject);
                App ap = new App();
                var appId = RedisService.Add<App>(ap);
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
                RedisService.AddCustomPropertyFor<App, CustomProperty>(ap.Id, prop);


                var lcdDetails = AppStoreUIService.GetElementDetailList(AppConfigKey.LCD_ATTR_ID);
                foreach (var lcd in lcdDetails)
                {
                    SetLCD(ap.Id, lcd.Value.ToString());
                }

                AppStoreUIService.SetAppForAppList<AppProject>(appProjectId, appSetting);
                #endregion

                #region Set up app project
                var originalAppProject = CloneHelper.DeepClone<AppProject>(appProject);
                appProject.AppNo = "tencent_" + app.appid;
                appProject.Creator = app.cpname;
                appProject.LogoFile = GetFileNameFromUri(appInfo.logo);
                appProject.Name = appInfo.name;
                appProject.PackageName = appInfo.packageName;
                appProject.Rate = appInfo.star.ToFloat();
                RedisService.UpdateWithRebuildIndex<AppProject>(originalAppProject, appProject);
                #endregion

                #region Set up App
                var originalApp = CloneHelper.DeepClone<App>(ap);
                var originalApp2 = RedisService.Get<App>(ap.Id);

                ap.AppNo = appProject.AppNo;
                ap.AppProjectId = appProject.Id;
                ap.UseGreaterVersion = true;
                ClientImageInfo lg = new ClientImageInfo
                {
                    BelongsToAppId = ap.Id,
                    FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(appInfo.logo)),
                    TypeId = "1"
                };
                RedisService.Add<ClientImageInfo>(lg);
                ap.ClientLogos = new List<ClientImageInfo> 
                            {
                                lg
                            };

                ImageInfo lg2 = new ImageInfo
                {
                    BelongsToAppId = ap.Id,
                    FileUrl = Path.Combine(LogoDirRoot, GetFileNameFromUri(appInfo.logo))
                };
                RedisService.Add<ImageInfo>(lg2);
                ap.Logo = lg2;

                ap.Name = appInfo.name;
                ap.OrderNumber = appInfo.downnum;
                ap.DownloadTimes = appInfo.downnum;
                ap.Status = 1;
                foreach (var s in appInfo.images)
                {
                    ImageInfo ss = new ImageInfo
                    {
                        BelongsToAppId = ap.Id,
                        FileUrl = Path.Combine(ScreenshotDirRoot, GetFileNameFromUri(s))
                    };
                    RedisService.Add<ImageInfo>(ss);
                    ap.ScreenShot.Add(ss);
                }
                ap.PlatformType = AppConfigKey.PLATFORM_TYPE_ID.ConfigValue().ToInt32();
                ap.Summary = appInfo.detail.Replace("<br/>", string.Empty).Replace("<br>", string.Empty);
                RedisService.UpdateWithRebuildIndex<App>(originalApp2, ap);
                #endregion

                #region Set up App Version
                if (!string.IsNullOrEmpty(appInfo.apkurl))
                {
                    FileInfo fi = new FileInfo(Path.Combine(APK_Folder_Base, GetFileNameFromUri(appInfo.apkurl)));

                    if (fi != null && fi.Exists)
                    {
                        AppVersion ver = new AppVersion
                        {
                            FileSize = (int)fi.Length,
                            FileUrl = GetFileNameFromUri(appInfo.apkurl),
                            PublishDateTime = appInfo.updatetime,
                            Status = 1,
                            VersionName = appInfo.apkver,
                            Id = appInfo.versionCode.ToString()
                        };
                        RedisService.SetSubModel<App, AppVersion>(ap.Id, ver);
                        AppStoreUIService.SetAppCurrentTestVersion(appId, ver.Id);
                        AppStoreUIService.PublishAppVersion(appId);

                        AndroidPackageView apkInfo = FileService.GetAndroidPackageInfomation(fi.FullName);
                        apkInfo.Id = ver.Id;
                        RedisService.SetSubModel<App, AndroidPackageView>(ap.Id, apkInfo);
                    }
                }
                #endregion

                #region Set up tags
                if (appInfo.type.StartsWith("soft", StringComparison.OrdinalIgnoreCase))
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
                AppStoreUIService.AddTagForAppProject(appInfo.category, appProject.Id);
                AddMarketTag(appInfo.category, ap.Id);
                AppStoreUIService.AddTagForApp("Live", ap.Id);
                AppStoreUIService.AddTagForApp("Valid", ap.Id);
                AppStoreUIService.AddTagForAppProject("From_tencent", appProject.Id);
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
                LogHelper.WriteInfo(string.Format("This AppProject {0} will delete, appProjectId is {1}", appProject.Name, appProject.Id));
                AppProjectDelete(appProject.Id);

            }
            return appProject;
        }

        private void AddNewVersionApp(ReformApp reformApp, AppListItem app, AppProject appProject)
        {
            try
            {
                var appitems = AppStoreUIService.GetAppsFromAppList<AppProject>(appProject.Id);
                foreach (var a in appitems)
                {
                    var versions = RedisService.GetAllSubModelIdsByType<App, AppVersion>(a.Id).ToIdsWithNoPrefix<AppVersion>();

                    if (!versions.Contains(app.versionCode.ToString()))
                    {
                        reformApp.NewVersionCount = AddNewVersionForApp(reformApp.NewVersionCount, app, a);
                    }
                    else
                    {
                        reformApp.DupVersionCount++;
                        LogHelper.WriteInfo(string.Format("Already has version {1} for app name {0}", app.name, app.versionCode), ConsoleColor.DarkYellow);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message + ex.StackTrace);
            }
        }

        public void AppUpdate(AppProject appProject)
        {
            var apps = AppStoreUIService.GetAppsFromAppList<AppProject>(appProject.Id);
            if (apps != null)
            {
                foreach (var app in apps)
                {
                    var originalApp = RedisService.Get<TargetApp>(app.Id);
                    originalApp.CategoryId = "1";
                    originalApp.CategoryName = "游戏";
                    originalApp.NameLowCase = originalApp.Name.ToLower();
                    RedisService.UpdateWithRebuildIndex<TargetApp>(originalApp, originalApp);
                }
            }
        }

        private int AddNewVersionForApp(int newVersionCount, TencentAppStoreModel.AppListItem app, App a)
        {
            newVersionCount++;

            #region Checked TYD App
            var tags = AppStoreUIService.GetTagsByApp(a.Id);
            // if the app has "TYD手动维护" tag, skip it.
            if (tags.FindIndex(x => (x.Name == AppConfigKey.TAG_TYD_SKIP) || (x.Id == AppConfigKey.TAG_TYD_SKIP_ID)) != -1)
            {
                LogHelper.WriteInfo(string.Format("TYD手动维护 -- {0}, skipped", app.name), ConsoleColor.Yellow);

                return newVersionCount;
            }

            LogHelper.WriteInfo(string.Format("Has new version for app, name {0}, downloading...", app.name), ConsoleColor.Yellow);
            #endregion

            #region Set up App Version
            AppInfo appInfo = DowloadAppInfoAndResources(app.appid.ToString());

            if (!string.IsNullOrEmpty(appInfo.apkurl))
            {
                FileInfo fi = new FileInfo(Path.Combine(APK_Folder_Base, GetFileNameFromUri(appInfo.apkurl)));

                if (fi != null && fi.Exists)
                {
                    AppVersion ver = new AppVersion
                    {
                        FileSize = (int)fi.Length,
                        FileUrl = GetFileNameFromUri(appInfo.apkurl),
                        PublishDateTime = appInfo.updatetime,
                        Status = 1,
                        VersionName = appInfo.apkver,
                        Id = appInfo.versionCode.ToString()
                    };
                    var originalApp = RedisService.Get<App>(a.Id);
                    AndroidPackageView apkInfo = FileService.GetAndroidPackageInfomation(fi.FullName);
                    apkInfo.Id = ver.Id;
                    if (a.Status == 0)
                    {
                        RedisService.UpdateWithRebuildIndex<App>(originalApp, a);
                        LogHelper.WriteInfo(string.Format("This App {0} status is invaild", a.Name), ConsoleColor.Gray);
                    }

                    RedisService.SetSubModel<App, AppVersion>(a.Id, ver);
                    RedisService.SetSubModel<App, AndroidPackageView>(a.Id, apkInfo);
                    AppStoreUIService.SetAppCurrentTestVersion(a.Id, ver.Id);
                    AppStoreUIService.PublishAppVersion(a.Id);
                }
            }

            #endregion

            #region Set up tags
            if (a.Status != 0)
                AppStoreUIService.AddTagForApp("Valid", a.Id);
            AppStoreUIService.AddTagForApp(appInfo.category, a.Id);
            AddMarketTag(appInfo.category, a.Id);
            AppStoreUIService.AddTagForApp("From_tencent", a.Id);
            AppStoreUIService.AddTagForApp("Live", a.Id);
            #endregion

            return newVersionCount;
        }
        #endregion

        #region Get All App Info

        public List<TencentAppStoreModel.AppListItem> GetAllAppInfos()
        {
            List<TencentAppStoreModel.AppListItem> allApps = new List<TencentAppStoreModel.AppListItem>();
            var categories = GetAllCategories();

            foreach (var c in categories)
            {
                var apps = GetAppInfosByCategoryID(c.categoryid.ToString());
                LogHelper.WriteInfo(string.Format("Category {0} has {1} apps", c.name, apps.Count), ConsoleColor.Yellow);
                allApps.AddRange(apps);
            }

            return allApps;
        }

        #region Get All Categories

        private List<TencentAppStoreModel.Category> GetAllCategories()
        {
            CategoryList categoryList = new CategoryList();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["type"] = "1";
            string resultJsonApp = categoryList.GetLiveData(parameters);
            parameters["type"] = "2";
            string resultJsonGame = categoryList.GetLiveData(parameters);

            return GetAllCategories(resultJsonApp, resultJsonGame);
        }

        private List<TencentAppStoreModel.Category> GetAllCategories(string resultJsonApp, string resultJsonGame)
        {
            List<TencentAppStoreModel.Category> ret = new List<TencentAppStoreModel.Category>();

            var appCategories = GetTencentModelBase(resultJsonApp);
            var gameCategories = GetTencentModelBase(resultJsonGame);

            ret = GetTencentCategory(appCategories, ret);
            ret = GetTencentCategory(gameCategories, ret);

            return ret;
        }

        private TencentModelBase GetTencentModelBase(string resultJson)
        {
            try
            {
                var categories = JsonConvert.DeserializeObject<TencentModelBase>(resultJson.MakeSureNotNull());
                if (categories.status == 0)
                {
                    categories.info = categories.info == null ? string.Empty : categories.info;
                    categories.info = JsonConvert.DeserializeObject<List<TencentAppStoreModel.Category>>(categories.info.ToString());
                }
                return categories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<TencentAppStoreModel.Category> GetTencentCategory(TencentModelBase model, List<TencentAppStoreModel.Category> ret)
        {
            if (model != null)
            {
                var appret = model.info as List<TencentAppStoreModel.Category>;
                if (appret != null)
                {
                    ret = ret.Union(appret).ToList();
                }
            }
            return ret;
        }

        #endregion

        public List<TencentAppStoreModel.AppListItem> GetAppInfosByCategoryID(string categoryID)
        {
            TencentAppStoreCap.Connectable.Category categoryProxy = new TencentAppStoreCap.Connectable.Category();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["categoryid"] = categoryID;
            parameters["page_size"] = "5";
            parameters["page_no"] = "1";
            parameters["page_index"] = "1";
            var appInfosJson = categoryProxy.GetLiveData(parameters);

            var appInfos = JsonConvert.DeserializeObject<TencentPagedModelBase>(appInfosJson.MakeSureNotNull());
            if (appInfos.status == 0)
            {
                parameters["categoryid"] = categoryID;
                parameters["page_size"] = appInfos.total.ToString();
                parameters["page_no"] = "1";
                parameters["page_index"] = "1";
                appInfosJson = categoryProxy.GetLiveData(parameters);
                appInfos = JsonConvert.DeserializeObject<TencentPagedModelBase>(appInfosJson.MakeSureNotNull());

                if (appInfos.status == 0)
                {
                    appInfos.info = appInfos.info == null ? string.Empty : appInfos.info;
                    appInfos.info = JsonConvert.DeserializeObject<List<TencentAppStoreModel.AppListItem>>(appInfos.info.ToString());
                }
            }

            return appInfos.info as List<TencentAppStoreModel.AppListItem>;
        }

        public AppInfo DowloadAppInfoAndResources(string appId = "96138")
        {
            AppInfo app = new AppInfo();
            AppDetail appProxy = new AppDetail();
            var request = new UrlRequest();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["appid"] = appId;

            var appDetailJson = appProxy.GetLiveData(parameters);
            var appInfoModel = JsonConvert.DeserializeObject<TencentModelBase>(appDetailJson.MakeSureNotNull());

            if (appInfoModel.status == 0)
            {
                appInfoModel.info = appInfoModel.info == null ? string.Empty : appInfoModel.info;
                appInfoModel.info = JsonConvert.DeserializeObject<List<AppInfo>>(appInfoModel.info.ToString());
            }

            var appInfos = appInfoModel.info as List<AppInfo>;
            if (appInfos != null && appInfos.Count > 0)
            {
                MakeSureDIRExist(APK_Folder_Base);
                MakeSureDIRExist(Screenshots_Folder_Base);
                MakeSureDIRExist(Logo_Folder_Base);

                app = appInfos[0];
                foreach (var img in app.images)
                {
                    request.DownloadFile(img, Path.Combine(Screenshots_Folder_Base, GetFileNameFromUri(img)));
                }

                request.DownloadFile(app.logo, Path.Combine(Logo_Folder_Base, GetFileNameFromUri(app.logo)));
                request.DownloadFile(app.apkurl, Path.Combine(APK_Folder_Base, GetFileNameFromUri(app.apkurl)));
            }

            return app;
        }

        #endregion

        private bool IsSoftware(List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                if (tag.Name.EqualsOrdinalIgnoreCase("社交") ||
                    tag.Name.EqualsOrdinalIgnoreCase("系统") ||
                    tag.Name.EqualsOrdinalIgnoreCase("安全") ||
                    tag.Name.EqualsOrdinalIgnoreCase("工具") ||
                    tag.Name.EqualsOrdinalIgnoreCase("通讯") ||
                    tag.Name.EqualsOrdinalIgnoreCase("娱乐") ||
                    tag.Name.EqualsOrdinalIgnoreCase("音乐") ||
                    tag.Name.EqualsOrdinalIgnoreCase("美化") ||
                    tag.Name.EqualsOrdinalIgnoreCase("视频") ||
                    tag.Name.EqualsOrdinalIgnoreCase("阅读") ||
                    tag.Name.EqualsOrdinalIgnoreCase("生活") ||
                    tag.Name.EqualsOrdinalIgnoreCase("摄影") ||
                    tag.Name.EqualsOrdinalIgnoreCase("教育") ||
                    tag.Name.EqualsOrdinalIgnoreCase("健康") ||
                    tag.Name.EqualsOrdinalIgnoreCase("新闻") ||
                    tag.Name.EqualsOrdinalIgnoreCase("办公") ||
                    tag.Name.EqualsOrdinalIgnoreCase("旅游") ||
                    tag.Name.EqualsOrdinalIgnoreCase("购物"))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddMarketTag(string category, string appId)
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

        private void MakeSureDIRExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private string GetFileNameFromUri(string uriPath)
        {
            Uri uri = new Uri(uriPath);
            return uri.AbsolutePath.Replace("/", "_");
        }

        private void SetLCD(string appId, string lcd)
        {
            var existedLcdSettings = RedisService.GetCustomPropertyFrom<App, CustomProperty>(appId, "2");
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
                    Id = "2",
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

        public void AppProjectDeleteWithEmpty(string appProjectId)
        {
            var tags = AppStoreUIService.GetTagsByAppProject(appProjectId);
            if (tags != null)
            {
                try
                {
                    foreach (var t in tags)
                    {
                        AppStoreUIService.DeleteTagFromAppProject(t.Id, appProjectId);
                    }
                }
                catch (Exception)
                {
                    var appProject_e = RedisService.Get<AppProject>(appProjectId);
                    RedisService.Delete<AppProject>(appProject_e);
                }
            }

            var appProject = RedisService.Get<AppProject>(appProjectId);
            RedisService.Delete<AppProject>(appProject);
        }

        public void AppProjectDelete(string appProjectId)
        {
            var tags = AppStoreUIService.GetTagsByAppProject(appProjectId);
            if (tags != null)
            {
                foreach (var t in tags)
                {
                    AppStoreUIService.DeleteTagFromAppProject(t.Id, appProjectId);
                }
            }
            var apps = this.AppStoreUIService.GetAppsFromAppList<AppProject>(appProjectId);

            if (apps != null)
            {
                foreach (var app in apps)
                {
                    var appTags = AppStoreUIService.GetTagsByApp(app.Id);
                    if (appTags != null)
                    {
                        foreach (var t in appTags)
                        {
                            AppStoreUIService.DeleteTagForApp(t.Id, app.Id);
                        }
                    }
                    DeleteRedundanceForAppBranch(app.Id);
                    RedisService.DeleteWithCustomProperties<App, CustomProperty>(app.Id);
                    DeleteAppSettingForAppColumn(app.Id);
                }
            }

            var appColumnIds = RedisService.GetAllActiveModelIds<AppColumn>();

            //delete LogoFile 
            RedisService.DeleteWithCustomProperties<AppProject, CustomProperty>(appProjectId);
        }

        public void AppVersionDelete(string appProjectId, AppListItem item)
        {

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

        private void DeleteAppSettingForAppColumn(string AppId)
        {
            var appColumnIds = RedisService.GetAllActiveModelIds<AppColumn>();
            foreach (var appColumnId in appColumnIds)
            {
                var appSettings = RedisService.GetAllSubModelsByType<AppColumn, AppSettingsForAppList>(appColumnId);
                foreach (var appSetting in appSettings)
                {
                    if (appSetting.Id == AppId)
                    {
                        AppStoreUIService.DeleteAppFromAppList<AppColumn>(appColumnId, AppId);
                    }
                }
            }
        }

        #endregion
    }
}
