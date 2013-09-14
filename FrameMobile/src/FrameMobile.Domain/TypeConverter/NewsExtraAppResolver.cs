using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Domain.Service;
using FrameMobile.Model.News;
using StructureMap;

namespace FrameMobile.Domain
{
    public class NewsExtraAppResolver : ValueResolver<TouTiaoContentModel, int>
    {
        protected override int ResolveCore(TouTiaoContentModel source)
        {
            var dbContextService = ObjectFactory.GetInstance<IDbContextService>();
#if DEBUG
            return 1;
#else
            var extraAppList = dbContextService.Find<NewsExtraApp>(x => x.Status == 1);

            return extraAppList.RandomInt();
#endif
        }
    }
}
