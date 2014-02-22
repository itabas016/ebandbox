using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpapertopic")]
    public class WallPaperTopic : MySQLModel
    {
        [SubSonicStringLength(128)]
        [SubSonicNullString]
        public string TopicLogoUrl { get; set; }

        public int OrderNumber { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Summary { get; set; }

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Comment { get; set; }
    }
}
