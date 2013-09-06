using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class NewsSubCategoryView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayname")]
        public string DisplayName { get; set; }

        [JsonProperty("sourceid")]
        public int SourceId { get; set; }

        [JsonProperty("categoryid")]
        public int CategoryId { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("cursor")]
        public long Cursor { get; set; }
    }
}
