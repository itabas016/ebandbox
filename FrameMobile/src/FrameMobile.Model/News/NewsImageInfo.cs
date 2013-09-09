using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsimageinfo")]
    public class NewsImageInfo : MySQLModelBase
    {
        public long NewsId { get; set; }

        //1 为大图，2为中图，3为小图
        public int Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [SubSonicStringLength(512)]
        public string URL { get; set; }
    }
}
