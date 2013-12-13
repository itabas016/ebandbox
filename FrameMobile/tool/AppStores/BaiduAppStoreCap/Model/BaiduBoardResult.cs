using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlType("board")]
    public class BaiduBoardResult
    {
        /// <summary>
        /// 榜单名称
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// 榜单记录总数
        /// </summary>
        [XmlAttribute("disp_num")]
        public string Total { get; set; }

        /// <summary>
        /// 当前页记录数
        /// </summary>
        [XmlAttribute("ret_num")]
        public string Count { get; set; }

        /// <summary>
        /// 每页显示结果数 最大值20
        /// </summary>
        [XmlAttribute("rn")]
        public string PageSize { get; set; }

        /// <summary>
        /// 搜索偏移量 为rn的整数倍
        /// </summary>
        [XmlAttribute("pn")]
        public string PageNum { get; set; }

        /// <summary>
        /// 应用列表结果
        /// </summary>
        [XmlArray("apps")]
        public List<BaiduApp> AppList { get; set; }
    }
}
