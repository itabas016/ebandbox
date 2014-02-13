using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Radar
{
    [Serializable]
    [SubSonicTableNameOverride("radarcategory")]
    public class RadarCategory : MySQLModel
    {
        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string NormalLogoUrl { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string HDLogoUrl { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
