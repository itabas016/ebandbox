using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Radar
{
    [Serializable]
    [SubSonicTableNameOverride("subradar")]
    public class SubRadar : MySQLModel
    {
        public int RadarId { get; set; }

        [SubSonicStringLength(256)]
        public string Comment { get; set; }
    }
}
