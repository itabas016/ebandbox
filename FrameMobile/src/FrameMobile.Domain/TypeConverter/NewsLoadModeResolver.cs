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
    public class NewsLoadModeResolver : ValueResolver<TouTiaoContentModel, int>
    {
        protected override int ResolveCore(TouTiaoContentModel source)
        {
            var dbContextService = ObjectFactory.GetInstance<IDbContextService>();

            var loadmodelist = dbContextService.Find<NewsLoadMode>(x => x.Status == 1);

            return loadmodelist.RandomInt();
        }
    }
}
