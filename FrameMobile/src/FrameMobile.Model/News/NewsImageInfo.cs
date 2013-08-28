using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("NewsImageInfo")]
    public class NewsImageInfo : MySQLModelBase
    {
        public int NewsId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [SubSonicStringLength(512)]
        public string URL { get; set; }
    }
}
