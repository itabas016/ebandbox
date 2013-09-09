using SubSonic.SqlGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newsadvert")]
    public class NewsAdvert : MySQLModelBase
    {
        [SubSonicStringLength(64)]
        public string Name { get; set; }

        [SubSonicStringLength(64)]
        public string NameLowCase { get; set; }

        [SubSonicStringLength(128)]
        public string PackageName { get; set; }
    }
}
