using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QihooAppStoreCap.Model
{
    [Serializable]
    public class QihooAppStoreGetAppResult : QihooAppStoreResultBase
    {
        /// <summary>
        /// 应用列表
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public List<QihooAppStoreApp> QihooApplist { get; set; }

    }
}
