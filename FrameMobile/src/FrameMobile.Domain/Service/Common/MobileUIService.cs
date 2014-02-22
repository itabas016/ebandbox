using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.Mobile;
using FrameMobile.Domain;

namespace FrameMobile.Domain.Service
{
    public class MobileUIService : ThemeDbContextService, IMobileUIService
    {
        public IList<MobileBrand> GetMobileBrandList()
        {
            var brandlist = dbContextService.All<MobileBrand>().ToList();
            return brandlist;
        }

        public IList<MobileHardware> GetMobileHardwareList()
        {
            var hardwarelist = dbContextService.All<MobileHardware>().ToList();
            return hardwarelist;
        }

        public IList<MobileResolution> GetMobileResolutionList()
        {
            var resolutionlist = dbContextService.All<MobileResolution>().ToList();
            return resolutionlist;
        }

        public IList<MobileProperty> GetMobilePropertyList()
        {
            var propertylist = dbContextService.All<MobileProperty>().ToList();
            return propertylist;
        }
    }
}
