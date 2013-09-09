using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class TouTiaoContentView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("newsid")]
        public long NewsId { get; set; }

        [JsonProperty("sourceid")]
        public int SourceId { get; set; }

        [JsonProperty("categoryid")]
        public int? CategoryId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("wapurl")]
        public string WAPURL { get; set; }

        [JsonProperty("appopenurl")]
        public string AppOpenURL { get; set; }

        [JsonProperty("publishtime")]
        public DateTime PublishTime { get; set; }

        [JsonProperty("imageurl")]
        public string ImageURL { get; set; }

        [JsonProperty("advertpkg")]
        public string AdvertPkgName { get; set; }
    }
}
