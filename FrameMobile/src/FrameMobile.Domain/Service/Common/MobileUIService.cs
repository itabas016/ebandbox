using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Model.Mobile;
using FrameMobile.Domain;
using FrameMobile.Common;

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

        public IList<MobileChannel> GetMobileChannelList()
        {
            var channellist = dbContextService.All<MobileChannel>().ToList();
            return channellist;
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
                        }).GroupBy(x => x.Value).Select(x => x.First());
            return lcds.ToList();
        }

        public IList<MobileProperty> GetSimilarMobilePropertyList(int currentWidth, decimal similarRatio)
        {
            var propertylist = from p in dbContextService.Find<MobileProperty>(x => x.Status == 1)
                               join r in dbContextService.Find<MobileResolution>(x => x.SimilarRatio == similarRatio && x.Status == 1) on p.ResolutionId equals r.Id
                               where r.Value.GetResolutionWidth() <= currentWidth
                               select new MobileProperty()
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   BrandId = p.BrandId,
                                   ResolutionId = p.ResolutionId
                               };
            return propertylist.ToList();
        }

        public MobileChannel GetMobileChannel(int channelId)
        {
            var channel = dbContextService.Single<MobileChannel>(x => x.Id == channelId && x.Status == 1);
            return channel;
        }

        public MobileChannel GetMobileChannel(string channelName)
        {
            var channel = dbContextService.Single<MobileChannel>(x => x.Value == channelName.ToLower() && x.Status == 1);
            return channel;
        }
    }
}
