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

namespace QihooAppStoreCap
{
    public class AppItemCap
    {
        #region prop

        private GetApps _app;
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

        public void BuildAppProject(ReformApp reformApp, QihooAppStoreApp appItem)
        {
            var appProject = AppStoreUIService.GetAppProjectByPKGName(appItem.PackageName);

            if (appProject == null)
            {
                LogHelper.WriteInfo(string.Format("Has new app, name {0}, downloading...", appItem.Name), ConsoleColor.Yellow);
                reformApp.NewAppCount++;

                appProject = AddNewApp(appItem, appProject);
            }
        }

        public AppProject AddNewApp(QihooAppStoreApp appItem, AppProject appProject)
        {
            var app = new App();

            appProject = SetupAppList(appProject, out app);

            appProject = SetupAppProject(appItem, appProject);

            app = SetupApp(appItem, appProject, app);

            SetupTags(appItem, appProject, app);

            return appProject;
        }

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

            appProject.AppNo = "tencent_" + appItem.Id;
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
            var screenShotlist = appItem.ScreenHotsURL.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
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
                FileInfo fi = new FileInfo(Path.Combine(APK_Folder_Base, GetFileNameFromUri(appItem.DownloadURL)));

                if (fi != null && fi.Exists)
                {
                    AppVersion ver = new AppVersion
                    {
                        FileSize = (int)fi.Length,
                        FileUrl = GetFileNameFromUri(appItem.DownloadURL),
                        PublishDateTime = appItem.UpdateTime.ToExactDateTime("yyyy-MM-dd"),
                        Status = 1,
                        VersionName = appItem.VersionName,
                        Id = appItem.VersionCode
                    };
                    RedisService.SetSubModel<App, AppVersion>(app.Id, ver);
                    AppStoreUIService.SetAppCurrentTestVersion(app.Id, ver.Id);
                    AppStoreUIService.PublishAppVersion(app.Id);

                    AndroidPackageView apkInfo = FileService.GetAndroidPackageInfomation(fi.FullName);
                    apkInfo.Id = ver.Id;
                    RedisService.SetSubModel<App, AndroidPackageView>(app.Id, apkInfo);
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
            AppStoreUIService.AddTagForAppProject(appItem.CategoryName, appProject.Id);
            AddMarketTag(appItem.CategoryName, app.Id);
            AppStoreUIService.AddTagForApp("Live", app.Id);
            AppStoreUIService.AddTagForApp("Valid", app.Id);
            AppStoreUIService.AddTagForAppProject("From_tencent", appProject.Id);
        }

        #endregion

        #region Helper

        private string GetFileNameFromUri(string uriPath)
        {
            Uri uri = new Uri(uriPath);
            return uri.AbsolutePath.Replace("/", "_");
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

        #endregion
    }
}
