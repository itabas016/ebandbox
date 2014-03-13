using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using FrameMobile.Model.AppStore;
using FrameMobile.Model.News;
using NCore;
using Newtonsoft.Json;

namespace FrameMobile.Domain.Service
{
    public class CommonService : NewsDbContextService
    {
        public void UpdateNews(DateTime startTime, DateTime endTime)
        {
            var contentlist = new List<NewsContent>();
            var limit = ConfigKeys.UPDATE_NEWS_SAME_PUBLISH_TIME_LIMIT.ConfigValue().ToInt32();

            var dupPublishTimeList = (from l in
                                          dbContextService.Find<NewsContent>(x => x.PublishTime >= startTime && x.PublishTime <= endTime)
                                      group l by l.PublishTime into gp
                                      where gp.Count() >= limit
                                      select gp.Key);
            if (dupPublishTimeList != null && dupPublishTimeList.Count() > 0)
            {
                foreach (var item in dupPublishTimeList)
                {
                    var retlist = from l in
                                      dbContextService.Find<NewsContent>(x => x.PublishTime == item)
                                  select l;
                    contentlist = contentlist.Union(retlist).ToList();
                }
            }

            if (contentlist != null && contentlist.Count > 0)
            {
                foreach (var item in contentlist)
                {
                    var random = new Random(Guid.NewGuid().GetHashCode());

                    item.PublishTime = item.PublishTime.AddSeconds(random.Next(1, 60 * 60));
                }

                var ret = dbContextService.Update<NewsContent>(contentlist);

                NLogHelper.WriteTrace(string.Format("Update NewsContent {0} rows from {1} to {2}.", ret, startTime, endTime));
            }
            else
            {
                NLogHelper.WriteTrace(string.Format("No rows update!"));
            }
        }

        public void UpdateNewsExtraApp(string packageName = "")
        {
            var extraapplist = string.IsNullOrEmpty(packageName) ?
                dbContextService.All<NewsExtraApp>().ToList() : dbCommonContextService.Find<NewsExtraApp>(x => x.PackageName == packageName).ToList();

            var updateTimes = 0;
            foreach (var item in extraapplist)
            {
                var ret = UpdateExtraAppSingle(item);
                updateTimes = updateTimes + ret;
            }
            if (updateTimes > 0)
            {
                var config = dbContextService.Single<NewsConfig>(x => x.NameLowCase == "newsextraapp");
                config.Version++;
                var r = dbContextService.Update<NewsConfig>(config);
                if (r > 0)
                {
                    NLogHelper.WriteTrace(string.Format("the extra app config version update success, current version is {0} !", config.Version));
                }
            }

        }

        private int UpdateExtraAppSingle(NewsExtraApp extraApp)
        {
            var appDetail = GetAppDetail(extraApp.PackageName);

            if (!string.IsNullOrEmpty(appDetail.DownloadURL) && !string.IsNullOrEmpty(appDetail.VersionCode) && !string.IsNullOrEmpty(appDetail.PackageName))
            {
                var sb = new StringBuilder();
                var older_versionCode = extraApp.VersionCode;
                var latest_versionCode = appDetail.VersionCode.ToInt32();
                var downloadurl = extraApp.ExtraLinkUrl;

                if (older_versionCode != latest_versionCode || downloadurl != appDetail.DownloadURL)
                {
                    extraApp.ExtraLinkUrl = appDetail.DownloadURL;
                    extraApp.VersionCode = latest_versionCode;
                    sb.AppendFormat("the exta app name: {0} \r\n", extraApp.Name);
                    sb.AppendFormat("the exta app older version code: {0} \r\n", older_versionCode);
                    sb.AppendFormat("the exta app latest version code: {0} \r\n", latest_versionCode);
                    sb.AppendFormat("the exta app older download url: {0} \r\n", downloadurl);
                    sb.AppendFormat("the exta app latest download url: {0}", appDetail.DownloadURL);

                    NLogHelper.WriteTrace(sb.ToString());
                    var ret = dbContextService.Update<NewsExtraApp>(extraApp);
                    if (ret > 0)
                    {
                        NLogHelper.WriteTrace(string.Format("the extra app {0} is update success! \r\n", extraApp.Name));
                        return ret;
                    }
                    else
                    {
                        NLogHelper.WriteTrace(string.Format("the extra app {0} is update failed! \r\n", extraApp.Name));
                    }
                }
            }
            return 0;

        }

        private AppDetail GetAppDetail(string packageName)
        {
            var requestUrl = string.Format(ConfigKeys.NEW_MARKET_APPDETAIL_REQUEST_URL.ConfigValue(), packageName);
            var response = HttpHelper.GetData(requestUrl);

            if (!string.IsNullOrEmpty(response))
            {
                var appDetail = JsonConvert.DeserializeObject<AppDetail>(response);
                return appDetail;
            }
            return new AppDetail();
        }
    }
}
