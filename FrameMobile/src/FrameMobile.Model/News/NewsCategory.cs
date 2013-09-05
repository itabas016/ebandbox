using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.News
{
    [Serializable]
    [SubSonicTableNameOverride("newscategory")]
    public class NewsCategory : MySQLModelBase
    {
        [SubSonicStringLength(64)]
        public string Name { get; set; }
    }
}
