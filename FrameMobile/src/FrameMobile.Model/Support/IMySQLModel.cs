using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Model
{
    public interface IMySQLModel : IMySQLModelBase
    {
        string Name { get; set; }
    }
}
