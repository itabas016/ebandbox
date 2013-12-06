using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QihooAppStoreCap.Model
{
    [Serializable]
    public class QihooAppStoreCompleteApp : QihooAppStoreApp
    {
        /// <summary>
        /// large icon下载地址
        /// </summary>
        [JsonProperty(PropertyName = "larg_icon")]
        public string LargeIcon { get; set; }

        /// <summary>
        /// 分类父ID
        /// </summary>
        [JsonProperty(PropertyName = "categoryPid")]
        public string CategoryPId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [JsonProperty(PropertyName = "categoryId")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [JsonProperty(PropertyName = "recommendOrder")]
        public string RecommendOrder { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        [JsonProperty(PropertyName = "download_url")]
        public string DownloadURL { get; set; }

        /// <summary>
        /// 市场标识
        /// </summary>
        [JsonProperty(PropertyName = "market_id")]
        public string MarketId { get; set; }

        /// <summary>
        /// 应用签名
        /// </summary>
        [JsonProperty(PropertyName = "signature_md5")]
        public string SignatureMD5 { get; set; }

        /// <summary>
        /// 应用Key
        /// </summary>
        [JsonProperty(PropertyName = "app_key")]
        public string AppKey { get; set; }

        /// <summary>
        /// 应用大小，单位b
        /// </summary>
        [JsonProperty(PropertyName = "apk_size")]
        public string ApkSize { get; set; }

        /// <summary>
        /// Apk包的md5值
        /// </summary>
        [JsonProperty(PropertyName = "apk_md5")]
        public string ApkMD5 { get; set; }

        /// <summary>
        /// 应用入库时间
        /// </summary>
        [JsonProperty(PropertyName = "createTime")]
        public string CreateTime { get; set; }
    }
}
