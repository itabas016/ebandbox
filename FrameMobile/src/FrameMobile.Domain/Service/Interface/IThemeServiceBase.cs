using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public interface IThemeServiceBase
    {
        IList<ThemeConfigView> GetConfigViewList(MobileParam mobileParams, int type);

        MobileProperty GetMobileProperty(MobileParam mobileParams);
    }
}
