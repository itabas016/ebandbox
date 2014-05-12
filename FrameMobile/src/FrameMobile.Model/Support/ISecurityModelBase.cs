﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model
{
    public interface ISecurityModelBase : IMySQLModelBase
    {
        string JsonResult { get; set; }
        int Version { get; set; }
    }
}
