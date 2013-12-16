using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlType("category")]
    public class BaiduCategory
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// 分类Type
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
