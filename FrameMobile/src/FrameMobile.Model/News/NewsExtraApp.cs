using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsextraapp")]
    public class NewsExtraApp : NewsSource
    {
        //0 非浏览器 1 浏览器
        [SubSonicStringLength(64)]
        public int IsBrower { get; set; }

        [SubSonicStringLength(256)]
        public string DownloadURL { get; set; }
    }
}
