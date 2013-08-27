﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model.ThirdPart;
using FrameMobile.Model.TYDNews;

namespace FrameMobile.Domain
{
    public class TYDNewsMapping
    {
        internal static void CreateMap()
        {
            /*
            Mapper.CreateMap<DeviceModel, DeviceModelLog>().ForMember(dest => dest.Comments, opt => opt.MapFrom(ori => ori.Comment));
            Mapper.CreateMap<Element, ElementLog>();
            Mapper.CreateMap<ApplistItemView, ApplistItemMobileView>()
                .ForMember(dest => dest.DownLoadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore());
            */
            Mapper.CreateMap<TouTiaoContent, TouTiaoModel>()
                .ForMember(dest => dest.AppOpenURL, opt => opt.MapFrom(ori => ori.AppOpeURL))
                .ForMember(dest => dest.NewsId, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(ori => ori.Site))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Title))
                .ForMember(dest => dest.WAPURL, opt => opt.MapFrom(ori => ori.TouTiaoWAPURL))
                .ForMember(dest => dest.ImageList, opt => opt.MapFrom(ori => ori.ImageList));
        }
    }
}
