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

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string NormalURL { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string HDURL { get; set; }
    }
}
