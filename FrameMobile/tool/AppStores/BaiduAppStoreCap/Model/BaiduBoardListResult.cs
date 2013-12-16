using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("response")]
    public class BaiduBoardListResult : BaiduResultBase
    {
        /// <summary>
        /// 请求结果
        /// </summary>
        [XmlArray("boards"), XmlArrayItem("board")]
        public List<BaiduBoard> BoardList { get; set; }
    }
}
