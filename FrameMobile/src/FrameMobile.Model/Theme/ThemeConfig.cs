using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("themeconfig")]
    public class ThemeConfig : MySQLModel
    {
        [SubSonicStringLength(64)]
        public string NameLowCase { get; set; }

        //1 代表壁纸、2 代表铃声
        public int Type { get; set; }

        public int Version { get; set; }

        [SubSonicStringLength(256)]
        public string Comment { get; set; }
    }
}
