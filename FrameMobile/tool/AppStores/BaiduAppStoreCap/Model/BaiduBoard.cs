using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("board")]
    public class BaiduBoard
    {
        /// <summary>
        /// 榜单Id
        /// </summary>
        [XmlElement("id")]
        public int Id { get; set; }

        /// <summary>
        /// 榜单名称
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
