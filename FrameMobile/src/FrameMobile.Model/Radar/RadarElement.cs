using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Radar
{
    [Serializable]
    [SubSonicTableNameOverride("radarelement")]
    public class RadarElement : MySQLModel
    {
        [SubSonicStringLength(32)]
        [SubSonicNullString]
        public string RadarCategoryIds { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
