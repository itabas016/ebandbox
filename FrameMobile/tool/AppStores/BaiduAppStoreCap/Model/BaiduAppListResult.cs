using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    public class BaiduAppListResult
    {
        /// <summary>
        /// 榜单名称
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 榜单记录总数
        /// </summary>
        [XmlElement("disp_num")]
        public string Total { get; set; }

        /// <summary>
        /// 当前页记录数
        /// </summary>
        [XmlElement("ret_num")]
        public string Count { get; set; }

        /// <summary>
        /// 每页显示结果数 最大值20
        /// </summary>
        [XmlElement("rn")]
        public string PageSize { get; set; }

        /// <summary>
        /// 搜索偏移量 为rn的整数倍
        /// </summary>
        [XmlElement("pn")]
        public string PageNum { get; set; }

        /// <summary>
        /// 应用列表结果
        /// </summary>
        [XmlArray("apps")]
        public List<BaiduApp> AppList { get; set; }
    }
}
