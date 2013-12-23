using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TencentThumbnails
    {
        [JsonProperty(PropertyName = "qqnews_thu_big")]
        public string BigThumbailUrl { get; set; }

        [JsonProperty(PropertyName = "qqnews_thu")]
        public string ThumbnailUrl { get; set; }
    }
}
