using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model
{
    [Serializable]
    public class MobilePropertyBase : MySQLModel
    {
        [SubSonicStringLength(512)]
        public string Value { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
