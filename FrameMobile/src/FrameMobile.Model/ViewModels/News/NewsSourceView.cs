using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class NewsSourceView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [ViewField(IsDisplay = false)]
        [JsonProperty("namelowcase")]
        public string NameLowCase { get; set; }
        
        [JsonProperty("pkgname")]
        public string PackageName { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
