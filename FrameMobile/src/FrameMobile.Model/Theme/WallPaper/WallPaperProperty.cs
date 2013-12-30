using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaperproperty")]
    public class WallPaperProperty : MySQLModelBase
    {
        public int WallPaperId { get; set; }

        public int MobilePropertyId { get; set; }
    }
}
