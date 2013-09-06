using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FrameMobile.Web
{
    public class JsonResultBase
    {
        public JsonResultBase(CommonActionResult commonActionResult)
        {
            if (commonActionResult.CommonResult != null)
            {
                this.ResultCode = commonActionResult.CommonResult.result;

                this.Description = commonActionResult.CommonResult.desc;

                this.CustomResultHeaders = commonActionResult.CustomResultHeaders;
            }

            this.Host = commonActionResult.Host;
        }

        [JsonProperty(PropertyName = "result", Order = 0)]
        public int ResultCode { get; set; }

        [JsonProperty(PropertyName = "desc", Order = 10)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "host", Order = 20)]
        public string Host { get; set; }

        [JsonIgnore]
        public List<CustomHeaderItem> CustomResultHeaders { get; set; }

        public override string ToString()
        {
            var ret = JsonConvert.SerializeObject(this, Formatting.None, new IsoDateTimeConverter() { DateTimeFormat = DateTimeFormat.COMMON_TO_SECOND });

            if (CustomResultHeaders.Count > 0)
            {
                const string DATA = ",\"data\"";
                var dataIndex = ret.IndexOf(DATA);
                if (dataIndex > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var cItem in CustomResultHeaders)
                    {
                        if (cItem.IsValueType) sb.AppendFormat(",\"{0}\":{1}", cItem.Key, cItem.Value);
                        else sb.AppendFormat(",\"{0}\":\"{1}\"", cItem.Key, cItem.Value);
                    }

                    ret = string.Format("{0}{1}{2}", ret.Substring(0, dataIndex), sb.ToString(), ret.Substring(dataIndex));
                }
            }

            return ret;
        }
    }
}
