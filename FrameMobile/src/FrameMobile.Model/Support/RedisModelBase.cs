using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisMapper;

namespace FrameMobile.Model
{
    [Serializable]
    public class RedisModelBase : IRedisModel
    {
        [QueryOrSortField]
        public string Id { get; set; }

        private DateTime _createDateTime = DateTime.Now;
        [QueryOrSortField]
        public DateTime CreateDateTime
        {
            get
            {
                return _createDateTime;
            }
            set
            {
                _createDateTime = value;
            }
        }

        [QueryOrSortField]
        public string ModuleName { get; set; }
    }
}
