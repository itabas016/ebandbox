using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TouTiaoImageInfo
    {
        //图片宽度
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        //图片高度
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        //图片URLS
        [JsonProperty(PropertyName = "urls")]
        public List<string> UrlList { get; set; }

    }
}
