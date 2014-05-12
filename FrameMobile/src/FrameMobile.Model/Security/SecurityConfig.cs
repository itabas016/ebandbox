using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Security
{
    [Serializable]
    [SubSonicTableNameOverride("securityconfig")]
    public class SecurityConfig : MySQLModelBase
    {
        [SubSonicStringLength(128)]
        public string Name { get; set; }

        [SubSonicStringLength(64)]
        public string Type { get; set; }

        public int LatestVersion { get; set; }
    }
}
