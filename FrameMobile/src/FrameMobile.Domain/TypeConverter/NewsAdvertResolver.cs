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
    public class NewsAdvertResolver : ValueResolver<NewsContent, string>
    {
        protected override string ResolveCore(NewsContent source)
        {
            var dbContextService = ObjectFactory.GetInstance<IDbContextService>();

            var advertlist = dbContextService.Find<NewsAdvert>(x => x.Status == 1);
            var advert = dbContextService.Single<NewsAdvert>(x => x.Id == advertlist.RandomInt());
            return advert.PackageName;
        }
    }
}
