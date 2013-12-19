using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("result")]
    public class BaiduAppListResult : BaiduAppListResultBase
    {
        /// <summary>
        /// 应用列表结果
        /// </summary>
        [XmlArray("apps"), XmlArrayItem("app")]
        public List<BaiduApp> AppList { get; set; }
    }
}
