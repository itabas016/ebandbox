using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QihooAppStoreCap.Model
{
    [Serializable]
    public class QihooAppStoreCategory
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [JsonProperty(PropertyName = "categoryId")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 分类PId
        /// </summary>
        [JsonProperty(PropertyName = "categoryPid")]
        public string CategoryPId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [JsonProperty(PropertyName = "categoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类 Brief
        /// </summary>
        [JsonProperty(PropertyName = "categoryBrief")]
        public string CategoryBrief { get; set; }

        /// <summary>
        /// 分类图片地址Banner1
        /// </summary>
        [JsonProperty(PropertyName = "categoryBanner1")]
        public string CategoryBanner1 { get; set; }

        /// <summary>
        /// 分类图片地址Banner2
        /// </summary>
        [JsonProperty(PropertyName = "categoryBanner2")]
        public string CategoryBanner2 { get; set; }
    }
}
