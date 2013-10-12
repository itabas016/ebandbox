using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newssource")]
    public class NewsSource : MySQLModel
    {
        [SubSonicStringLength(64)]
        public string NameLowCase { get; set; }

        [SubSonicStringLength(128)]
        public string PackageName { get; set; }
    }
}
