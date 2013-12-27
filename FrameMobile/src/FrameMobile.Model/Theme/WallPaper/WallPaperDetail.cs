using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaperdetail")]
    public class WallPaperDetail : WallPaper
    {
        public int WallPaperId { get; set; }

        [SubSonicStringLength(512)]
        public string OriginalUrl { get; set; }

        public DateTime ModifiedTime
        {
            get { return modifiedTime; }
            set { modifiedTime = value; }
        } private DateTime modifiedTime = DateTime.Now;

        [SubSonicStringLength(512)]
        [SubSonicNullString]
        public string Comment { get; set; }

        //预留字段
        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Ex1 { get; set; }

        [SubSonicStringLength(256)]
        [SubSonicNullString]
        public string Ex2 { get; set; }
    }
}
