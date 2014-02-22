using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class NewsExtraResult
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("sver")]
        public int ServerViersion { get; set; }

        [JsonProperty("extralist")]
        public List<NewsExtraAppView> NewsExtraList { get; set; }
    }
}
