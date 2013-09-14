using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsconfig")]
    public class NewsConfig : MySQLModelBase
    {
        [SubSonicStringLength(64)]
        public string Name { get; set; }

        [SubSonicStringLength(64)]
        public int Version { get; set; }
    }
}
