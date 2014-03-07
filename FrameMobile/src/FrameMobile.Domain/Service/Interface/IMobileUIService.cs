using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public interface IMobileUIService
    {
        IList<MobileBrand> GetMobileBrandList();
        IList<MobileHardware> GetMobileHardwareList();
        IList<MobileResolution> GetMobileResolutionList();
        IList<MobileChannel> GetMobileChannelList();
        IList<MobileProperty> GetMobilePropertyList();
        IList<MobileResolution> GetMobileResolutionList(List<int> mobilePropertyIds);
    }
}
