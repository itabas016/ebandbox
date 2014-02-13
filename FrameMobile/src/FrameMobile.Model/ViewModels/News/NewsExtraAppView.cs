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
    [SubSonicTableNameOverride("newsextraapp")]
    public class NewsExtraAppView : NewsSourceView
    {
        [JsonProperty("isbrower")]
        public int IsBrower { get; set; }

        [JsonProperty("type")]
        public int ShowType { get; set; }

        [JsonProperty("logourl")]
        public string ExtraAppLogoUrl { get; set; }

        [JsonProperty("downloadurl")]
        public string DownloadURL { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
