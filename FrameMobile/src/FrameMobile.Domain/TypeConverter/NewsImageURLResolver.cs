using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model.News;

namespace FrameMobile.Domain
{
    public class NewsImageURLResolver : ValueResolver<TouTiaoContentModel, string>
    {
        protected override string ResolveCore(TouTiaoContentModel source)
        {
            throw new NotImplementedException();
        }
    }
}
