using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model
{
    [Serializable]
    public class MySQLModel : MySQLModelBase
    {
        [SubSonicStringLength(64)]
        public string Name { get; set; }
    }
}
