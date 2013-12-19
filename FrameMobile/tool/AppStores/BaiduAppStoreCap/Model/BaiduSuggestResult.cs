using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("response")]
    public class BaiduSuggestResult : BaiduResultBase
    {
        /// <summary>
        /// 应用列表结果
        /// </summary>
        [XmlArray("sugs"), XmlArrayItem("sug")]
        public List<BaiduSugKey> Keys { get; set; }
    }
}
