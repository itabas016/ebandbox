using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QihooAppStoreCap.Model
{
    [Serializable]
    public class QihooAppStoreGetOffineAppResult : QihooAppStoreResultBase
    {
        [JsonProperty(PropertyName = "items")]
        public List<string> PackageNameList { get; set; }
    }
}
