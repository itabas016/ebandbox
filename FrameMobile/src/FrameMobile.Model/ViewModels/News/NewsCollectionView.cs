using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FrameMobile.Model
{
    public class NewsCollectionView : ViewModelBase
    {
        [JsonProperty("extraresult")]
        public NewsExtraResult NewsExtraResult { get; set; }

        [JsonProperty("contentresult")]
        public NewsContentResult NewsContentResult { get; set; }
    }
}
