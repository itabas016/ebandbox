using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using QihooAppStoreCap;
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
    public class AppItemCap : AppItemCapBase
    {
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

            var appResult = _service.DeserializeBase<QihooAppStoreGetAppResult>(data);

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
            if (page >= 1)
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
                LogHelper.WriteInfo(string.Format("AppProjectId: {0} is delete.", appProject.Id));
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

        #region Setup

        public void SetupTags(QihooAppStoreApp appItem, AppProject appProject, App app)
        {
            SetupTagsByCategoryName(appItem, appProject, app);
        }

        public void SetupTags(QihooAppStoreApp appItem, App app)
        {
            SetupTagsByCategoryName(appItem, app);
        }

        #endregion

    }
}
