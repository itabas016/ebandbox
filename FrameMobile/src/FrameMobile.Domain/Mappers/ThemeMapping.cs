using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model;
using FrameMobile.Model.Theme;

namespace FrameMobile.Domain
{
    public class ThemeMapping
    {
        internal static void CreateMap()
        {
            #region ViewModel

            #region Config

            Mapper.CreateMap<ThemeConfig, ThemeConfigView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.NameLowCase, opt => opt.MapFrom(ori => ori.NameLowCase))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(ori => ori.Type))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(ori => ori.Version))
                .IgnoreAllNonExisting();

            #endregion

            #region WallPaper

            Mapper.CreateMap<WallPaper, WallPaperView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Titile))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(ori => ori.ThumbnailName))
                .ForMember(dest => dest.OriginalUrl, opt => opt.MapFrom(ori => ori.OriginalName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(ori => ori.Rating))
                .ForMember(dest => dest.DownloadNumber, opt => opt.MapFrom(ori => ori.DownloadNumber))
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(ori => ori.PublishTime))
                .ForMember(dest => dest.ModifiedTime, opt => opt.MapFrom(ori => ori.ModifiedTime))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(ori => ori.OrderNumber))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<WallPaperCategory, WallPaperCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(ori => ori.CategoryLogoUrl))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(ori => ori.OrderNumber))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));

            Mapper.CreateMap<WallPaperSubCategory, WallPaperSubCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(ori => ori.CategoryId))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(ori => ori.SubCategoryLogoUrl))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(ori => ori.OrderNumber))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));

            Mapper.CreateMap<WallPaperTopic, WallPaperTopicView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(ori => ori.TopicLogoUrl))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(ori => ori.OrderNumber))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));

            #endregion

            #endregion
        }
    }
}
