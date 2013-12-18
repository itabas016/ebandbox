using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("app")]
    public class BaiduApp
    {
        /// <summary>
        /// 应用唯一Id
        /// </summary>
        [XmlElement("docid")]
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [XmlElement("sname")]
        public string Name { get; set; }

        /// <summary>
        /// 应用分类名
        /// </summary>
        [XmlElement("catename")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 资源包大小
        /// </summary>
        [XmlElement("packagesize")]
        public string Size { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [XmlElement("realsedate")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 资源包的下载地址
        /// </summary>
        [XmlElement("url")]
        public virtual string DownloadUrl { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        [XmlElement("icon")]
        public string IconUrl { get; set; }

        /// <summary>
        /// 资源包的版本号
        /// </summary>
        [XmlElement("versionname")]
        public string VersionName { get; set; }
    }
}
