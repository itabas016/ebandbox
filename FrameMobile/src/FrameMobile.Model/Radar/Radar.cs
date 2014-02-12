using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Radar
{
    [Serializable]
    [SubSonicTableNameOverride("radar")]
    public class Radar : MySQLModel
    {
        [SubSonicStringLength(256)]
        public string Comment { get; set; }
    }
}
