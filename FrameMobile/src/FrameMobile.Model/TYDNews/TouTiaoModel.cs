using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.ThirdPart;
using RedisMapper;

namespace FrameMobile.Model.TYDNews
{
    [Serializable]
    public class TouTiaoModel : RedisModelBase
    {
        public long NewsId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Site { get; set; }

        public string WAPURL { get; set; }

        public string AppOpenURL { get; set; }

        public List<TouTiaoImageInfo> ImageList { get; set; }

        [QueryOrSortField]
        public int Status { get; set; }
    }
}
