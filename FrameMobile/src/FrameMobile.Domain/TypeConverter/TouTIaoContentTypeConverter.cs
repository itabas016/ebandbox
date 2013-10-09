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
    public class TouTIaoContentTypeConverter : ITypeConverter<NewsContent, NewsContentView>
    {
        public NewsContentView Convert(ResolutionContext context)
        {
            var source = context.SourceValue as NewsContent;

            if (context.SourceValue == null) return null;

            var dbContextService = ObjectFactory.GetInstance<IDbContextService>();

            var dest = this.Convert(source, dbContextService);

            return dest;
        }

        public NewsContentView Convert(NewsContent model, IDbContextService service)
        {

            var loadmodelist = service.Find<NewsExtraApp>(x => x.Status == 1);

            return null;
        }
    }
}
