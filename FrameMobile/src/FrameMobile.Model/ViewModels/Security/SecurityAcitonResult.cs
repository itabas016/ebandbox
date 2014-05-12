using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    [Serializable]
    public class SecurityAcitonResult
    {
        [JsonProperty(PropertyName = "result")]
        public int Result { get; set; }

        [JsonProperty(PropertyName = "config")]
        public SecurityConfigData ConfigData { get; set; }

        public override string ToString()
        {
            if (this.ConfigData.Data == null)
            {
                this.Result = -2;
                return "{\"result\":-2}";
            }
            var ret = JsonConvert.SerializeObject(this, Formatting.None);

            ret = ret.Replace("\"[", "[").Replace("]\"", "]");
            return ret;
        }
    }
}
