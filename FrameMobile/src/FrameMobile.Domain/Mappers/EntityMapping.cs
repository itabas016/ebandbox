using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace FrameMobile.Domain
{
    public class EntityMapping
    {
        public static void Config()
        {
            NewsMapping.CreateMap();

            ThemeMapping.CreateMap();

            AccountMapping.CreateMap();

            Mapper.AssertConfigurationIsValid();
        }

        public static void ResetMapper()
        {
            Mapper.Reset();
        }

        public static T2 Auto<T1, T2>(T1 source, T2 defaultT2Value = default(T2))
        {
            if (source == null)
            {
                if (defaultT2Value != null) return defaultT2Value;
            }

            return Mapper.Map<T1, T2>(source);
        }

        public static object Map<T1>(T1 source, Type destType)
        {
            return Mapper.Map(source, typeof(T1), destType);
        }

        public static T2 Assign<T1, T2>(T1 source, T2 dest)
        {
            return Mapper.Map<T1, T2>(source, dest);
        }
    }
}
