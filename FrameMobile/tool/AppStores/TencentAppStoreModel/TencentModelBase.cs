using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TencentAppStoreModel
{
    public class TencentModelBase
    {
        public int status { get; set; }

        public string commandtype { get; set; }

        public object info { get; set; }
    }
}
