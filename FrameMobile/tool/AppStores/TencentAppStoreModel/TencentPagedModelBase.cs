using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TencentAppStoreModel
{
    public class TencentPagedModelBase : TencentModelBase
    {
        public int status { get; set; }

        public string commandtype { get; set; }

        public int pageno { get; set; }
        public int pages { get; set; }
        public int pagesize { get; set; }
        public int total { get; set; }
        public int nextpageindex { get; set; }
    }
}
