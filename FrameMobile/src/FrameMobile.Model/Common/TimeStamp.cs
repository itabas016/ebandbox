using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model.Common
{
    public class TimeStamp
    {
        public DateTime InputTime { get; set; }

        public long OutputStamp { get; set; }

        public long InputStamp { get; set; }

        public DateTime OutputTime { get; set; }
    }
}
