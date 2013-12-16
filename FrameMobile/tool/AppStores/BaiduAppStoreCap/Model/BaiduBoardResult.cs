﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlType("response")]
    public class BaiduBoardResult : BaiduResultBase
    {
        /// <summary>
        /// 榜单结果
        /// </summary>
        [XmlElement("result")]
        public BaiduAppListResult Result { get; set; }
    }
}
