using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TouTiaoCursor
    {
        //当前游标
        [JsonProperty(PropertyName = "cursor")]
        public long Cursor { get; set; }

        //Content
        [JsonProperty(PropertyName = "data")]
        public List<TouTiaoContent> ContentList { get; set; }
    }
}
