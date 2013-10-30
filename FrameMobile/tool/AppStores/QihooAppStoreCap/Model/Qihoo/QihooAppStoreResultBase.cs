using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QihooAppStoreCap.Model
{
    [Serializable]
    public class QihooAppStoreResultBase
    {
        /// <summary>
        /// 总数
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// 起始偏移量
        /// </summary>
        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        /// <summary>
        /// 获取应用个数
        /// </summary>
        [JsonProperty(PropertyName = "num")]
        public int Num { get; set; }

        /// <summary>
        /// 获取应用设置的起始时间时间戳
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public long StartTime { get; set; }

    }
}
