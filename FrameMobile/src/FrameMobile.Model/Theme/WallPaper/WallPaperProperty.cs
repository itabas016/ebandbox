using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaperproperty")]
    public class WallPaperProperty : MySQLModel
    {
        public int BrandId { get; set; }

        public int HardwareId { get; set; }

        public int ResoulutionId { get; set; }
    }
}
