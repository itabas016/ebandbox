﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace FrameMobile.Domain
{
    public static class NewsExtensions
    {
        public static TOutput To<TOutput>(this object source)
        {
            if (source == null ) return default(TOutput);

            return (TOutput)Mapper.Map(source, source.GetType(), typeof(TOutput));
        }

        public static IList<TOutput> To<TOutput>(this IList<object> source)
        {
            if (source == null) return default(IList<TOutput>);

            return (IList<TOutput>)Mapper.Map(source, source.GetType(), typeof(TOutput));
        }

        public static int RandomInt<T>(this IList<T> source)
        {
            if (source == null) return default(int);
            var random = new Random();
            return random.Next(0, source.Count);
        }
    }
}
