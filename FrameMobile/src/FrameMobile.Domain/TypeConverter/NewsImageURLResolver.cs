using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model.News;

namespace FrameMobile.Domain
{
    public class NewsImageURLResolver : ValueResolver<NewsContent, string>
    {
        protected override string ResolveCore(NewsContent source)
        {
            throw new NotImplementedException();
        }
    }
}
