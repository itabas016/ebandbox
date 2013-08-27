using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisMapper;

namespace FrameMobile.Model.TYDNews
{
    [Serializable]
    public class TYDNewsCategory : RedisModelBase
    {
        [QueryOrSortField]
        public string Name { get; set; }

        [QueryOrSortField]
        public int Status { get; set; }

        public bool? canEdit { get; set; }
    }
}
