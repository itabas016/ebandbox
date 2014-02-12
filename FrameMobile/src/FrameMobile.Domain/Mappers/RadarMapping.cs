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
            Mapper.CreateMap<Radar, RadarView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<SubRadar, SubRadarView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.RadarId, opt => opt.MapFrom(ori => ori.RadarId))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));
        }
    }
}
