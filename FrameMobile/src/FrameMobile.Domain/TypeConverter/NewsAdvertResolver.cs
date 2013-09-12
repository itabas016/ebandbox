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
    public class NewsAdvertResolver : ValueResolver<TouTiaoContentModel, string>
    {
        protected override string ResolveCore(TouTiaoContentModel source)
        {
            var dataBaseService = ObjectFactory.GetInstance<IDataBaseService>();

            var advertlist = dataBaseService.Find<NewsAdvert>(x => x.Status == 1);
            var advert = dataBaseService.Single<NewsAdvert>(x => x.Id == advertlist.RandomInt());
            return advert.PackageName;
        }
    }
}
