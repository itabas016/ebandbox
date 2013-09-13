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

        [JsonProperty("downloadurl")]
        public string DownloadURL
        {
            get
            {
                if (!string.IsNullOrEmpty(PackageName))
                {
                    this.DownloadURL = string.Format(ConfigKeys.TYD_NEWS_APP_DOWNLOAD_PREFIX_URL.ConfigValue(), PackageName);
                }
                return this.DownloadURL;
            }
            set { value = this.DownloadURL; }
        }
    }
}
