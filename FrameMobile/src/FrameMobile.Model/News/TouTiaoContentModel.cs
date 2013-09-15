using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("toutiaocontent")]
    public class TouTiaoContentModel : MySQLModelBase
    {
        public long NewsId { get; set; }

        public int? CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Title { get; set; }

        [SubSonicStringLength(1000)]
        [SubSonicNullString]
        public string Summary { get; set; }

        //可存储文本url
        [SubSonicStringLength(8000)]
        [SubSonicNullString]
        public string Content { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Site { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string WAPURL { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string AppOpenURL { get; set; }

        public DateTime PublishTime { get; set; }

        //预留字段
        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Ex1 { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Ex2 { get; set; }
    }
}
