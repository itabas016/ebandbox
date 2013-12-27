using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class WallPaperDetailView : WallPaperView
    {
        [JsonProperty("wallpaperid")]
        [JsonIgnore]
        public int WallPaperId { get; set; }

        [JsonProperty("originalurl")]
        public string OriginalUrl { get; set; }

        [JsonProperty("modifiedtime")]
        [JsonIgnore]
        public DateTime ModifiedTime { get; set; }

        [JsonProperty("comment")]
        [JsonIgnore]
        public string Comment { get; set; }
    }
}
