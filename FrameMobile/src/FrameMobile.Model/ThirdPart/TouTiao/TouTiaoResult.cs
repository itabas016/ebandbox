using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TouTiaoResult
    {
        //返回值
        [JsonProperty(PropertyName = "ret")]
        public int ret { get; set; }

        //说明
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        //数据
        [JsonProperty(PropertyName = "data")]
        public TouTiaoCursor DataByCursor { get; set; }
    }
}
