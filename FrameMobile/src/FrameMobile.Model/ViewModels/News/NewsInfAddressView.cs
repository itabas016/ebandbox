using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model
{
    [Serializable]
    public class NewsInfAddressView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sourceid")]
        public int SourceId { get; set; }

        [JsonProperty("categoryid")]
        public int CategoryId { get; set; }

        [JsonProperty("subcategoryid")]
        [JsonIgnore]
        public int SubCategoryId { get; set; }

        [JsonProperty("isstamp")]
        public int IsStamp { get; set; }

        [JsonProperty("infaddress")]
        public string InfAddress { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
