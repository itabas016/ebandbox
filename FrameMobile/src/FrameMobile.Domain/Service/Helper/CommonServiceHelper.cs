using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Model;
using FrameMobile.Model.Mobile;

namespace FrameMobile.Domain.Service
{
    public class CommonServiceHelper : CommonDbContextService, ICommonServiceHelper
    {
        [ServiceCache(ClientType = RedisClientManagerType.MixedCache)]
        public MobileProperty GetMobileProperty(MobileParam mobileParams)
        {
            var brand = mobileParams.Manufacturer.ToLower();
            brand = brand == Const.BRAND_KOOBEE ? Const.BRAND_KOOBEE : Const.BRAND_PCBA;
            var resolution = mobileParams.Resolution.ToLower();

            var mobileproperty = from p in dbContextService.All<MobileProperty>()
                                 join b in dbContextService.All<MobileBrand>() on p.BrandId equals b.Id
                                 join r in dbContextService.All<MobileResolution>() on p.ResolutionId equals r.Id
                                 where b.Value == brand && r.Value == resolution
                                 select new MobileProperty
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     BrandId = p.BrandId,
                                     ResolutionId = p.ResolutionId,
                                     HardwareId = p.HardwareId,
                                     Status = p.Status
                                 };
            return (MobileProperty)mobileproperty.SingleOrDefault<MobileProperty>().MakeSureNotNull();
        }
    }
}
