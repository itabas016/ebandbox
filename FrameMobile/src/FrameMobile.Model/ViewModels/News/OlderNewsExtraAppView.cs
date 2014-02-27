using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using Newtonsoft.Json;
using SubSonic.SqlGeneration.Schema;
using NCore;

namespace FrameMobile.Model
{
    [Serializable]
    public class OlderNewsExtraAppView : ViewModelBase
    {
        [ViewField(IsDisplay = false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namelowcase")]
        public string NameLowCase { get; set; }

        [JsonProperty("isbrower")]
        public int IsBrower { get; set; }

        [JsonProperty("pkgname")]
        public string PackageName { get; set; }

        [JsonProperty("downloadurl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
