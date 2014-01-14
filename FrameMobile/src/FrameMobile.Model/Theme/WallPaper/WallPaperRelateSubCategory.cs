using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaperrelatesubcategory")]
    public class WallPaperRelateSubCategory : MySQLModelBase
    {
        public int WallPaperId { get; set; }

        public int SubCategoryId { get; set; }
    }
}
