using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    public class BaiduAppDetailListResult : BaiduAppListResultBase
    {
        /// <summary>
        /// 应用详情列表结果
        /// </summary>
        [XmlArray("apps"), XmlArrayItem("app")]
        public List<BaiduAppDetail> AppDetailList { get; set; }
    }
}
