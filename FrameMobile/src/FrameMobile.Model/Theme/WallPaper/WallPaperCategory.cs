using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpapercategory")]
    public class WallPaperCategory : MySQLModel
    {
        [SubSonicStringLength(128)]
        [SubSonicNullString]
        public string CategoryLogoUrl { get; set; }

        public int OrderNumber { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
