using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model
{
    [Serializable]
    [SubSonicTableNameOverride("user")]
    public class User : MySQLModelBase
    {
        [SubSonicStringLength(64)]
        public string Name { get; set; }

        [SubSonicStringLength(64)]
        public string Password { get; set; }

        [SubSonicStringLength(128)]
        public string Email { get; set; }

        [SubSonicStringLength(256)]
        public string Comment { get; set; }
    }
}
