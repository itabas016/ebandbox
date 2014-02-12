using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class RadarElementView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("radarcategoryids")]
        public List<int> RadarCategoryIds { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("comment")]
        [JsonIgnore]
        public string Comment { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
