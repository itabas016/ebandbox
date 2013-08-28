using FrameMobile.Model.ThirdPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Domain
{
    public static class NewsExtensions
    {
        public static TDest To<TDest>(this TouTiaoContent source)
        {
            return EntityMapping.Auto<TouTiaoContent, TDest>(source);
        }

        public static IList<TDest> To<TDest>(this IList<TouTiaoContent> source)
        {
            return EntityMapping.Auto<IList<TouTiaoContent>, IList<TDest>>(source);
        }

        public static TDest To<TDest>(this TouTiaoImageInfo source)
        {
            return EntityMapping.Auto<TouTiaoImageInfo, TDest>(source);
        }

        public static IList<TDest> To<TDest>(this IList<TouTiaoImageInfo> source)
        {
            return EntityMapping.Auto<IList<TouTiaoImageInfo>, IList<TDest>>(source);
        }
    }
}
