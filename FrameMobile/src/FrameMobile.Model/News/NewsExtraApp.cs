using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsextraapp")]
    public class NewsExtraApp : MySQLModel
    {
        /// <summary>
        /// 是否是浏览器
        /// 0 非浏览器 1 浏览器
        /// </summary>
        public int IsBrower { get; set; }

        /// <summary>
        /// 推广类型
        /// 0 推广链接 1 推广应用
        /// </summary>
        public int ExtraType { get; set; }

        [SubSonicStringLength(128)]
        [SubSonicNullString]
        public string PackageName { get; set; }

        public int VersionCode { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string ExtraLogoUrl { get; set; }

        [SubSonicStringLength(256)]
        public string ExtraLinkUrl { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Description { get; set; }
    }
}
