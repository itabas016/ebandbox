using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class RadarCategoryView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("normallogourl")]
        [JsonIgnore]
        public string NormalLogoUrl { get; set; }

        [JsonProperty("hdlogourl")]
        [JsonIgnore]
        public string HDLogoUrl { get; set; }

        [JsonProperty("comment")]
        [JsonIgnore]
        public string Comment { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
