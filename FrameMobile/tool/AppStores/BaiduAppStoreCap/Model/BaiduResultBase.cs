using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    public class BaiduResultBase
    {
        /// <summary>
        /// 请求状态码
        /// </summary>
        [XmlElement("statuscode")]
        public int StatusCode { get; set; }

        /// <summary>
        /// 请求状态说明
        /// </summary>
        [XmlElement("statusmessage")]
        public string StatusMessage { get; set; }
    }
}
