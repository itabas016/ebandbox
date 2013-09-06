using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;

namespace FrameMobile.Model
{
    public abstract class ViewModelBase : IViewModel
    {
        public virtual string ToViewModelString()
        {
            return ResultBuilder.BuildInstance(this);
        }
    }
}
