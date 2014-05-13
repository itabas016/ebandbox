using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    [Serializable]
    public class SecurityConfigData
    {
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public int Rate { get; set; }

        [JsonProperty(PropertyName = "apps")]
        public string Data { get; set; }

        [JsonIgnore]
        public List<SecurityAppData2> RealData
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Data))
                {
                    var ret = JsonConvert.DeserializeObject<List<SecurityAppData2>>(this.Data);

                    return ret;
                }

                return new List<SecurityAppData2>();
            }
        }
    }
}
