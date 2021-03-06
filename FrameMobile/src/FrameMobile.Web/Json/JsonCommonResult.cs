﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Web
{
    public class JsonCommonResult : JsonResultBase
    {
        public JsonCommonResult(CommonActionResult commonActionResult)
            : base(commonActionResult)
        {
            this.Data = commonActionResult.ViewModels;

            if (this.Data != null) { this.Count = commonActionResult.ViewModels.Count; }

            this.ServerVerison = commonActionResult.ServerVerison;
            this.Ratio = commonActionResult.Ratio;
            this.Total = commonActionResult.Total;

            if (this.ServerVerison.HasValue)
            {
                this.CustomResultHeaders.Add(new CustomHeaderItem { Key = "sver", Value = this.ServerVerison.ToString(), IsValueType = true });
            }

            if (this.Ratio.HasValue)
            {
                this.CustomResultHeaders.Add(new CustomHeaderItem { Key = "ratio", Value = this.Ratio.ToString(), IsValueType = true });
            }

            if (this.Total.HasValue)
            {
                this.CustomResultHeaders.Add(new CustomHeaderItem { Key = "total", Value = this.Total.ToString(), IsValueType = true });
            }
        }

        [JsonProperty(PropertyName = "count", Order = 20)]
        public int Count { get; set; }

        [JsonIgnore]
        public int? ServerVerison { get; set; }

        [JsonIgnore]
        public int? Ratio { get; set; }

        [JsonIgnore]
        public int? Total { get; set; }

        [JsonProperty(PropertyName = "data", Order = 100)]
        public IList<IViewModel> Data { get; set; }
    }
}
