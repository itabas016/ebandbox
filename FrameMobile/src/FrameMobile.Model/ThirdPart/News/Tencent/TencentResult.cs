using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TencentResult
    {
        //返回值
        [JsonProperty(PropertyName = "ret")]
        public int Ret { get; set; }

        //说明
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        [JsonProperty(PropertyName = "news")]
        public List<TencentContent> NewsList { get; set; }
    }
}
