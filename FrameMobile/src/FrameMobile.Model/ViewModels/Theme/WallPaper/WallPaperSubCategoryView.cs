﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class WallPaperSubCategoryView : WallPaperCategoryView
    {
        [JsonProperty("categoryid")]
        public int CategoryId { get; set; }
    }
}
