using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaperrelatetopic")]
    public class WallPaperRelateTopic : MySQLModelBase
    {
        public int WallPaperId { get; set; }

        public int TopicId { get; set; }
    }
}
