using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Core;
using FrameMobile.Model;

namespace FrameMobile.Web
{
    public static class ViewModelExtensions
    {
        public static string ToViewModelString(this IList<IViewModel> viewModelList)
        {
            return ResultBuilder.BuildInstanceList<IViewModel>(viewModelList);
        }
    }
}
