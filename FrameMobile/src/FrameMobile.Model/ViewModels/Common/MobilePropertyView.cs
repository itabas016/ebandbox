using FrameMobile.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model
{
    public class MobilePropertyView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brandid")]
        public int BrandId { get; set; }

        [JsonProperty("hardwareid")]
        public int HardwareId { get; set; }

        [JsonProperty("resolutionid")]
        public int ResoulutionId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
