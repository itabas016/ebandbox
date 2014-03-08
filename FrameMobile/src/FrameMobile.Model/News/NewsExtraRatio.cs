using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsextraratio")]
    public class NewsExtraRatio : MySQLModelBase
    {
        public int ChannelId { get; set; }

        public int Ratio { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
