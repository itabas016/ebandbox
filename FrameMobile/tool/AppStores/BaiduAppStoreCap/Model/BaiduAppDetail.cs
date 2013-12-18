using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BaiduAppStoreCap.Model
{
    [Serializable]
    [XmlRoot("app")]
    public class BaiduAppDetail : BaiduApp
    {
        /// <summary>
        /// 应用分类Id
        /// </summary>
        [XmlElement("cateid")]
        public string CategoryId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [XmlElement("versioncode")]
        public string VersionCode { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        [XmlElement("package")]
        public string PackageName { get; set; }

        /// <summary>
        /// 资源包的下载地址
        /// </summary>
        [XmlElement("download_url")]
        public override string DownloadUrl { get; set; }

        /// <summary>
        /// HDicon
        /// </summary>
        [XmlElement("iconhdpi")]
        public string IconHDpi { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [XmlElement("brief")]
        public string Summary { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; }

        /// <summary>
        /// 截图1
        /// </summary>
        [XmlElement("screenshot1")]
        public string ScreenShot1 { get; set; }

        /// <summary>
        /// 截图2
        /// </summary>
        [XmlElement("screenshot2")]
        public string ScreenShot2 { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        [XmlElement("score")]
        public int Score { get; set; }

        /// <summary>
        /// 权限说明
        /// </summary>
        [XmlElement("permission_cn")]
        public string Permission { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [XmlElement("sourcename")]
        public string SourceName { get; set; }

        /// <summary>
        /// 更新说明
        /// </summary>
        [XmlElement("changelog")]
        public string ChangeLog { get; set; }

        /// <summary>
        /// aladdin_flag
        /// </summary>
        [XmlElement("aladdin_flag")]
        public string AladdinFlag { get; set; }

        /// <summary>
        /// ADapi
        /// </summary>
        [XmlElement("adapi")]
        public string ADAPI { get; set; }

        /// <summary>
        /// 截图
        /// </summary>
        [XmlElement("screenshot")]
        public string ScreenShot { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [XmlElement("md5")]
        public string Md5 { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [XmlElement("updatetime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [XmlElement("buildtime")]
        public DateTime BuildTime { get; set; }
    }
}
