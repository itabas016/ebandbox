using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.SqlGeneration.Schema;

namespace FrameMobile.Model.Theme
{
    [Serializable]
    [SubSonicTableNameOverride("wallpaper")]
    public class WallPaper : MySQLModelBase
    {
        [SubSonicStringLength(64)]
        public string Titile { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public DateTime PublishTime { get; set; }

        public int Rating { get; set; }

        [SubSonicStringLength(512)]
        public string ThumbnailUrl { get; set; }

        public int DownloadNumber { get; set; }

        public int OrderNumber { get; set; }

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
