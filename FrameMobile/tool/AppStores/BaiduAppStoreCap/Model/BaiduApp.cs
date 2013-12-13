using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlType("app")]
    public class BaiduApp
    {
        /// <summary>
        /// 应用唯一Id
        /// </summary>
        [XmlAttribute("docid")]
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [XmlAttribute("sname")]
        public string Name { get; set; }

        /// <summary>
        /// 应用分类Id
        /// </summary>
        [XmlAttribute("cateid")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 应用分类名
        /// </summary>
        [XmlAttribute("catename")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 包名称
        /// </summary>
        [XmlAttribute("package")]
        public string PackageName { get; set; }

        /// <summary>
        /// 资源包大小
        /// </summary>
        [XmlAttribute("packagesize")]
        public string Size { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [XmlAttribute("realsedate")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 资源包的下载地址
        /// </summary>
        [XmlAttribute("url")]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        [XmlAttribute("icon")]
        public string IconUrl { get; set; }

        /// <summary>
        /// 资源包的版本号
        /// </summary>
        [XmlAttribute("versioncode")]
        public string VersionCode { get; set; }
    }
}
