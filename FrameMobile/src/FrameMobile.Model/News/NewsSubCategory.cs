using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newssubcategory")]
    public class NewsSubCategory : MySQLModel
    {
        [SubSonicStringLength(64)]
        public string DisplayName { get; set; }

        public int SourceId { get; set; }

        public int CategoryId { get; set; }

        public long Cursor { get; set; }
    }
}
