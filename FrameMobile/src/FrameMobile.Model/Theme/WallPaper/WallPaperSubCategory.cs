using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpapersubcategory")]
    public class WallPaperSubCategory : MySQLModel
    {
        public int CategoryId { get; set; }

        [SubSonicStringLength(128)]
        [SubSonicNullString]
        public string SubCategoryLogoUrl { get; set; }

        public int OrderNumber { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
