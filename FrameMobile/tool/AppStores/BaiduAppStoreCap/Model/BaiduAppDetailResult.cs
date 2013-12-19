using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    public class BaiduAppDetailResult : BaiduAppListResultBase
    {
        /// <summary>
        /// 应用详情列表结果
        /// </summary>
        [XmlElement("app")]
        public BaiduAppDetail AppDetail { get; set; }
    }
}
