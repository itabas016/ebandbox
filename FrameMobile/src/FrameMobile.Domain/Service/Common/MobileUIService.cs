using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.Mobile;
using FrameMobile.Domain;

namespace FrameMobile.Domain.Service
{
    public class MobileUIService : CommonDbContextService, IMobileUIService
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

        public IList<MobileResolution> GetMobileResolutionList(List<int> mobilePropertyIds)
        {
            var lcds = (from r in dbContextService.Find<MobileResolution>(x => x.Status == 1)
                       join p in dbContextService.Find<MobileProperty>(x => x.Status == 1) on r.Id equals p.ResolutionId
                       where mobilePropertyIds.Contains(p.Id)
                       select new MobileResolution
                       {
                           Name = r.Name,
                           Value = r.Value
                       }).Distinct();
            return lcds.ToList();
        }
    }
}
