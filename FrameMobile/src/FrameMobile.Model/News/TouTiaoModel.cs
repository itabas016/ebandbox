using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("TouTiaoContent")]
    public class TouTiaoModel : MySQLModelBase
    {
        public long NewsId { get; set; }

        public int CategoryId { get; set; }

        [SubSonicStringLength(512)]
        public string Title { get; set; }

        [SubSonicStringLength(1000)]
        public string Summary { get; set; }

        [SubSonicStringLength(256)]
        public string Site { get; set; }

        [SubSonicStringLength(512)]
        public string WAPURL { get; set; }

        [SubSonicStringLength(256)]
        public string AppOpenURL { get; set; }

        public DateTime PublishTime { get; set; }
    }
}
