using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("response")]
    public class BaiduCategoryListResult : BaiduResultBase
    {
        /// <summary>
        /// 请求结果
        /// </summary>
        [XmlArray("categories"), XmlArrayItem("category")]
        public List<BaiduCategory> CategoryList { get; set; }
    }
}
