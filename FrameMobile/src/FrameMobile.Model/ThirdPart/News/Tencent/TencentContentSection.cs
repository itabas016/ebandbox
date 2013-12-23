using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model.ThirdPart
{
    [Serializable]
    public class TencentContentSection
    {
        [JsonProperty(PropertyName = "type")]
        public int NewsSectionType { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string NewsSectionValue { get; set; }
    }
}
