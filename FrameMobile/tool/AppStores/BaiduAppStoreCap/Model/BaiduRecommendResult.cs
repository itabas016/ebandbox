﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("response")]
    public class BaiduRecommendResult : BaiduResultBase
    {
        /// <summary>
        /// 结果
        /// </summary>
        [XmlElement("result")]
        public BaiduAppListResult Result { get; set; }
    }
}
