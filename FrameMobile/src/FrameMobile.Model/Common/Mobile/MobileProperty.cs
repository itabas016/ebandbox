using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Mobile
{
    [Serializable]
    [SubSonicTableNameOverride("mobileproperty")]
    public class MobileProperty : MySQLModel
    {
        public int BrandId { get; set; }

        public int HardwareId { get; set; }

        public int ResoulutionId { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
