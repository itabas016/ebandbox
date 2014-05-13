using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model
{
    public interface ISecurityViewModel
    {
        string JsonResult { get; set; }
        int Version { get; set; }
        int Rate { get; set; }
    }
}
