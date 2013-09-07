using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model.ThirdPart;
using FrameMobile.Model.News;
using FrameMobile.Model;

namespace FrameMobile.Domain
{
    public class NewsMapping
    {
        internal static void CreateMap()
        {
            Mapper.CreateMap<TouTiaoContent, TouTiaoContentModel>()
                .ForMember(dest => dest.AppOpenURL, opt => opt.MapFrom(ori => ori.AppOpeURL))
                .ForMember(dest => dest.NewsId, opt => opt.MapFrom(ori => ori.NewsId))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(ori => ori.Site.DefaultValue()))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Title.DefaultValue()))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(ori => ori.Abstract.DefaultValue()))
                .ForMember(dest => dest.WAPURL, opt => opt.MapFrom(ori => ori.TouTiaoWAPURL))
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(ori => ori.PublishTime.UTCStamp()))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<TouTiaoImageInfo, NewsImageInfo>()
                .ForMember(dest => dest.Height, opt => opt.MapFrom(ori => ori.Height))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(ori => ori.Width))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<NewsCategory, NewsCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name));

            Mapper.CreateMap<NewsSubCategory, NewsSubCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(ori => ori.DisplayName))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(ori => ori.CategoryId))
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(ori => ori.SourceId))
                .ForMember(dest => dest.Cursor, opt => opt.MapFrom(ori => ori.Cursor));

            Mapper.CreateMap<TouTiaoContentModel, TouTiaoContentView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.NewsId, opt => opt.MapFrom(ori => ori.NewsId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(ori => ori.CategoryId))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(ori => ori.Site))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Title))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(ori => ori.Summary))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(ori => ori.Content))
                .ForMember(dest => dest.AppOpenURL, opt => opt.MapFrom(ori => ori.AppOpenURL))
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(ori => ori.PublishTime))
                .IgnoreAllNonExisting();
        }

    }
}
