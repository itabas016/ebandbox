using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model
{
    public interface IMySQLModelBase
    {
        int Id { get; set; }
        System.DateTime CreateDateTime { get; set; }
        int Status { get; set; }
    }
}
