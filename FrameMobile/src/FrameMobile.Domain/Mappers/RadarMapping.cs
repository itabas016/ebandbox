using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model;
using FrameMobile.Model.Radar;

namespace FrameMobile.Domain
{
    public class RadarMapping
    {
        internal static void CreateMap()
        {
            Mapper.CreateMap<RadarCategory, RadarCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<RadarElement, RadarElementView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.RadarCategoryIds, opt => opt.MapFrom(ori => ori.RadarCategoryIds.GetIds()))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));
        }
    }
}
