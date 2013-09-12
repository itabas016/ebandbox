using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Domain.Service;
using FrameMobile.Model;
using FrameMobile.Model.News;
using StructureMap;

namespace FrameMobile.Domain
{
    public class TouTIaoContentTypeConverter : ITypeConverter<TouTiaoContentModel, TouTiaoContentView>
    {
        public TouTiaoContentView Convert(ResolutionContext context)
        {
            var source = context.SourceValue as TouTiaoContentModel;

            if (context.SourceValue == null) return null;

            var dataBaseService = ObjectFactory.GetInstance<IDataBaseService>();

            var dest = this.Convert(source, dataBaseService);

            return dest;
        }

        public TouTiaoContentView Convert(TouTiaoContentModel model, IDataBaseService service)
        {

            var loadmodelist = service.Find<NewsLoadMode>(x => x.Status == 1);
            var advertlist = service.Find<NewsAdvert>(x => x.Status == 1);

            return null;
        }
    }
}
