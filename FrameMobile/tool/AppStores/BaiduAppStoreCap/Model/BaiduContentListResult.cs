using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlType("response")]
    public class BaiduContentListResult : BaiduResultBase
    {
        /// <summary>
        /// 结果
        /// </summary>
        [XmlElement("result")]
        public BaiduAppDetailListResult Result { get; set; }
    }
}
