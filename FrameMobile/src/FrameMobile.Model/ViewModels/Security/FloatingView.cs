using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model
{
    [Serializable]
    public class FloatingView : ISecurityViewModel
    {
        public string JsonResult { get; set; }

        public int Version { get; set; }

        public int Rate { get; set; }
    }
}
