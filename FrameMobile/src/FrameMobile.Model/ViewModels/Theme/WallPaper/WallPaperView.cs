﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class WallPaperView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("categoryid")]
        public int CategoryId { get; set; }

        [JsonProperty("subcategoryid")]
        public int SubCategoryId { get; set; }

        [JsonProperty("publishtime")]
        public DateTime PublishTime { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("thumbnailurl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("downloadnumber")]
        public int DownloadNumber { get; set; }

        [JsonProperty("ordernumber")]
        public int OrderNumber { get; set; }

        [JsonProperty("originalurl")]
        public string OriginalUrl { get; set; }

        [JsonProperty("modifiedtime")]
        [JsonIgnore]
        public DateTime ModifiedTime { get; set; }

        [JsonProperty("comment")]
        [JsonIgnore]
        public string Comment { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
