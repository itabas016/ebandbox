using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class NewsRadarView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logourl")]
        public string LogoUrl { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("radarelement")]
        public List<NewsRadarElementView> NewsRadarElementList { get; set; }
    }
}
