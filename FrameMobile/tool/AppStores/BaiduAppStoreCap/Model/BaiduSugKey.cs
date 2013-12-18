using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("sug")]
    public class BaiduSugKey
    {
        /// <summary>
        /// Suggest key
        /// </summary>
        [XmlElement("sug")]
        public string Key { get; set; }
    }
}
