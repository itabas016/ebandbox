using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class NewsRadarElementView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("radarid")]
        [JsonIgnore]
        public int RadarId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
