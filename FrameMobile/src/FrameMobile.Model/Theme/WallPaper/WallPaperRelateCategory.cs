using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaperrelatecategory")]
    public class WallPaperRelateCategory : MySQLModelBase
    {
        public int WallPaperId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }
    }
}
