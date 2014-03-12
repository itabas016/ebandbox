using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public interface ICommonServiceHelper
    {
        MobileProperty GetMobileProperty(MobileParam mobileParams);
    }
}
