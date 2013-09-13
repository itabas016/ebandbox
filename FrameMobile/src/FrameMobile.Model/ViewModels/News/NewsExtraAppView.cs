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
    [SubSonicTableNameOverride("newsextraapp")]
    public class NewsExtraAppView : NewsSourceView
    {
        [JsonProperty("isbrower")]
        public int IsBrower { get; set; }

        [JsonProperty("downloadurl")]
        public string DownloadURL { get; set; }
    }
}
