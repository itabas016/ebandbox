using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QihooAppStoreCap.Model
{
    [Serializable]
    public class QihooAppStoreApp
    {
        /// <summary>
        /// 软件在360市场的唯一标识
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// 市场标识
        /// </summary>
        [JsonProperty(PropertyName = "marketId")]
        public string MarketId { get; set; }

        /// <summary>
        /// 应用包名
        /// </summary>
        [JsonProperty(PropertyName = "packageName")]
        public string PackageName { get; set; }

        /// <summary>
        /// icon下载地址
        /// </summary>
        [JsonProperty(PropertyName = "iconUrl")]
        public string IconURL { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 应用分类名称
        /// </summary>
        [JsonProperty(PropertyName = "categoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 应用描述信息
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 截图地址
        /// </summary>
        [JsonProperty(PropertyName = "screenhotsUrl")]
        public string ScreenHotsURL { get; set; }

        /// <summary>
        /// 应用评分
        /// </summary>
        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        [JsonProperty(PropertyName = "downloadTimes")]
        public string DownloadTimes { get; set; }

        /// <summary>
        /// 作者/公司
        /// </summary>
        [JsonProperty(PropertyName = "developer")]
        public string Developer { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        [JsonProperty(PropertyName = "versionName")]
        public string VersionName { get; set; }

        /// <summary>
        /// 应用内部版本号
        /// </summary>
        [JsonProperty(PropertyName = "versionCode")]
        public string VersionCode { get; set; }

        /// <summary>
        /// 应用大小 单位Kb
        /// </summary>
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        /// <summary>
        /// 更新信息
        /// </summary>
        [JsonProperty(PropertyName = "updateInfo")]
        public string UpdateInfo { get; set; }

        /// <summary>
        /// 最近更新时间 2012-03-15
        /// </summary>
        [JsonProperty(PropertyName = "updateTime")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 最低系统要求
        /// </summary>
        [JsonProperty(PropertyName = "minVersion")]
        public string MinVersion { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        [JsonProperty(PropertyName = "downloadUrl")]
        public string DownloadURL { get; set; }

        /// <summary>
        /// apk包的Md5值
        /// </summary>
        [JsonProperty(PropertyName = "apkMd5")]
        public string ApkMD5 { get; set; }

        /// <summary>
        /// 是否有免费
        /// </summary>
        [JsonProperty(PropertyName = "isFree")]
        public bool IsFree { get; set; }

        /// <summary>
        /// 应用别名
        /// </summary>
        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; set; }

        /// <summary>
        /// 标签多个
        /// </summary>
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        /// <summary>
        /// 分辨率 为空
        /// </summary>
        [JsonProperty(PropertyName = "resolutions")]
        public string Resolutions { get; set; }

        /// <summary>
        /// 字段更新日期
        /// </summary>
        [JsonProperty(PropertyName = "descDate")]
        public string DescDate { get; set; }

        /// <summary>
        /// 是否beta 为空
        /// </summary>
        [JsonProperty(PropertyName = "isBeta")]
        public string IsBeta { get; set; }

        /// <summary>
        /// 市场的详细页 为空
        /// </summary>
        [JsonProperty(PropertyName = "marketUrl")]
        public string MarketURL { get; set; }

        /// <summary>
        /// 数字签名
        /// </summary>
        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }

        /// <summary>
        /// 是否有广告
        /// </summary>
        [JsonProperty(PropertyName = "isAd")]
        public bool IsAD { get; set; }

        /// <summary>
        /// 是否有推送
        /// </summary>
        [JsonProperty(PropertyName = "isPush")]
        public bool IsPush { get; set; }

        /// <summary>
        /// 是否有插件
        /// </summary>
        [JsonProperty(PropertyName = "isSdk")]
        public bool IsSDK { get; set; }

        /// <summary>
        /// 是否有积分墙
        /// </summary>
        [JsonProperty(PropertyName = "isIntegral")]
        public bool IsIntegral { get; set; }

        /// <summary>
        /// 应用是否安全 恒为1
        /// </summary>
        [JsonProperty(PropertyName = "isSecurity")]
        public bool IsSecurity { get; set; }
    }
}
