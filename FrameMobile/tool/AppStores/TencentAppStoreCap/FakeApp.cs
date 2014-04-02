using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using TYD.Mobile.Core;
using TYD.Mobile.Infrastructure.AppStore.Models;
using TYD.Mobile.Infrastructure.Domain.Services;
using TYD.Mobile.Infrastructure.Models.ViewModels.AppStores;
using RedisMapper;
using NCore;

namespace TencentAppStoreCap
{
    public class FakeApp
    {
        #region Ctor
        public IFileService FileService { get; set; }
        public IRedisService RedisService { get; set; }
        public IAppStoreUIService AppStoreUIService { get; set; }

        public FakeApp()
        {
            FileService = new FileService();
            RedisService = new RedisService();
            AppStoreUIService = new AppStoreUIService(FileService, RedisService);
        }

        public FakeApp(IFileService fileService, IRedisService redisService, IAppStoreUIService appStoreUIService)
        {
            this.FileService = fileService;
            this.RedisService = redisService;
            this.AppStoreUIService = appStoreUIService;
        }
        #endregion

        public void FixApkInfo()
        {
            var tencentAppIds = AppStoreUIService.GetAppIdsByTag(AppStoreUIService.GetTagIdByName("From_tencent")).ToIdsWithNoPrefix<App>();
            foreach (var id in tencentAppIds)
            {
                var verids = RedisService.GetAllSubModelIdsByType<App, AppVersion>(id).ToIdsWithNoPrefix<AppVersion>();
                foreach (var vid in verids)
                {
                    var apkInfo = RedisService.GetSubModel<App, AndroidPackageView>(id, vid);
                    if (apkInfo == null)
                    {
                        Console.WriteLine(string.Format("app {0} ver {1}", id, vid));
                    }
                }
            }
        }

        public void SetValidForTencentApps()
        {
            var tencentAppIds = AppStoreUIService.GetAppIdsByTag(AppStoreUIService.GetTagIdByName("From_tencent")).ToIdsWithNoPrefix<App>();
            int count = 0;
            foreach (var id in tencentAppIds)
            {
                AppStoreUIService.AddTagForApp("Valid", id);
                LogHelper.WriteInfo(string.Format("add valid tag for app id : {0}", id));
                count++;
            }
            LogHelper.WriteInfo(string.Format("tencent app count: {0}", count));
        }

        public void SetAppTypeForTencentApps()
        {
            var tencentAppIds = AppStoreUIService.GetAppIdsByTag(AppStoreUIService.GetTagIdByName("From_tencent")).ToIdsWithNoPrefix<App>();
            int count = 0;
            foreach (var id in tencentAppIds)
            {
                var tags = AppStoreUIService.GetTagsByApp(id);
                if (IsSoftware(tags))
                {
                    AppStoreUIService.AddTagForApp("Software", id);
                    AppStoreUIService.AddTagForApp("Top10Soft", id);
                }
                else
                {
                    AppStoreUIService.AddTagForApp("Game", id);
                    AppStoreUIService.AddTagForApp("Top10Games", id);
                }

                LogHelper.WriteInfo(string.Format("add type tag for app id : {0}", id));
                count++;
            }
            LogHelper.WriteInfo(string.Format("tencent app count: {0}", count));
        }

        private bool IsUsefulTag(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                List<string> validTags = new List<string> { "生活地图", "系统输入", "聊天通讯", "影音图像", "壁纸主题", "阅读学习", "网络邮件", "办公商务", "休闲益智", "体育竞技", "棋牌天地", "动作冒险", "飞行射击", "经营策略", "角色扮演", "网络游戏", "Game", "Software" };
                if (validTags.Contains(tag))
                {
                    return true;
                }
            }

            return false;
        }

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

        public void DeleteUnValidApkInfo()
        {
            List<KeyValuePair<string, string>> delAppVerInfos = new List<KeyValuePair<string, string>>();

            delAppVerInfos.Add(new KeyValuePair<string, string>(59025.ToString(), 8.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(59167.ToString(), 62.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(60785.ToString(), 560.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(59229.ToString(), 8.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(59231.ToString(), 1.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(61504.ToString(), 4.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(61538.ToString(), 1.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(61624.ToString(), 8.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(60923.ToString(), 19.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(60931.ToString(), 11.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(58779.ToString(), 12.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(59683.ToString(), 41.ToString()));
            delAppVerInfos.Add(new KeyValuePair<string, string>(60041.ToString(), 21.ToString()));

            RedisService redis = new RedisService();
            AppStoreUIService AppStoreUIService = new AppStoreUIService(FileService, redis);
            foreach (var del in delAppVerInfos)
            {
                redis.DeleteSubModel<App, AndroidPackageView>(del.Key, del.Value);
                redis.DeleteSubModel<App, AppVersion>(del.Key, del.Value);
                LogHelper.WriteInfo(string.Format("Remove App {0} with Version code {1}", del.Key, del.Value));
            }
        }

        public string GetData(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            return response.Content;
        }

        public byte[] GetDataBytes(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return response.RawBytes;
        }

        public string GetDataByGBK(string url)
        {
            var data = GetDataBytes(url);
            Encoding gb = Encoding.GetEncoding("gbk");
            return gb.GetString(data);
        }

        public string GetDataByUTF8(string url)
        {
            var data = GetDataBytes(url);
            Encoding utf8 = Encoding.UTF8;
            return utf8.GetString(data);
        }

    }
}
