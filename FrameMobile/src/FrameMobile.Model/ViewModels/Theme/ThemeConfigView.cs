using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class ThemeConfigView :ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namelowcase")]
        public string NameLowCase { get; set; }

        [JsonIgnore]
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("sver")]
        public string Version { get; set; }
    }
}
