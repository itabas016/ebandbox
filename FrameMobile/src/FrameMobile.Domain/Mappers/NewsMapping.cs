using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model.ThirdPart;
using FrameMobile.Model.News;
using FrameMobile.Model;
using FrameMobile.Model.Radar;

namespace FrameMobile.Domain
{
    public class NewsMapping
    {
        internal static void CreateMap()
        {
            #region Model

            Mapper.CreateMap<TouTiaoContent, NewsContent>()
                    .ForMember(dest => dest.AppOpenURL, opt => opt.MapFrom(ori => ori.AppOpeURL))
                    .ForMember(dest => dest.NewsId, opt => opt.MapFrom(ori => ori.NewsId))
                    .ForMember(dest => dest.Site, opt => opt.MapFrom(ori => ori.Site.DefaultValue()))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Title.DefaultValue()))
                    .ForMember(dest => dest.Summary, opt => opt.MapFrom(ori => ori.Abstract.DefaultValue()))
                    .ForMember(dest => dest.WAPURL, opt => opt.MapFrom(ori => ori.TouTiaoWAPURL))
                    .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(ori => ori.PublishTime.UTCStamp()))
                    .IgnoreAllNonExisting();

            Mapper.CreateMap<TouTiaoImageInfo, NewsImageInfo>()
                .IgnoreAllNonExisting();

            Mapper.CreateMap<TencentContent, NewsContent>()
                .ForMember(dest => dest.NewsId, opt => opt.MapFrom(ori => ori.NewsId.TruncLong()))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(ori => ori.Source.DefaultValue()))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Title.DefaultValue()))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(ori => ori.Summary.DefaultValue()))
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(ori => ori.Stamp.UTCStamp()))
                .IgnoreAllNonExisting();


            #endregion

            #region ViewModel

            Mapper.CreateMap<NewsConfig, NewsConfigView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.NameLowCase, opt => opt.MapFrom(ori => ori.NameLowCase))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(ori => ori.Version));

            Mapper.CreateMap<NewsSource, NewsSourceView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.NameLowCase, opt => opt.MapFrom(ori => ori.NameLowCase))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(ori => ori.PackageName));

            Mapper.CreateMap<NewsExtraApp, NewsExtraAppView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.NameLowCase, opt => opt.MapFrom(ori => ori.NameLowCase))
                .ForMember(dest => dest.IsBrower, opt => opt.MapFrom(ori => ori.IsBrower))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(ori => ori.PackageName))
                .ForMember(dest => dest.DownloadURL, opt => opt.MapFrom(ori => ori.DownloadURL));

            Mapper.CreateMap<NewsInfAddress, NewsInfAddressView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(ori => ori.SourceId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(ori => ori.CategoryId))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(ori => ori.SubCategoryId))
                .ForMember(dest => dest.IsStamp, opt => opt.MapFrom(ori => ori.IsStamp))
                .ForMember(dest => dest.InfAddress, opt => opt.MapFrom(ori => ori.InfAddress));

            Mapper.CreateMap<NewsCategory, NewsCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));

            Mapper.CreateMap<NewsSubCategory, NewsSubCategoryView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.NameLowCase, opt => opt.MapFrom(ori => ori.NameLowCase))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(ori => ori.CategoryId))
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(ori => ori.SourceId))
                .ForMember(dest => dest.Cursor, opt => opt.MapFrom(ori => ori.Cursor));

            Mapper.CreateMap<RadarCategory, NewsRadarView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<RadarElement, NewsRadarElementView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name))
                .ForMember(dest => dest.RadarId, opt => opt.MapFrom(ori => ori.RadarId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(ori => ori.Status));

            Mapper.CreateMap<NewsContent, NewsContentView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(ori => ori.Id))
                .ForMember(dest => dest.NewsId, opt => opt.MapFrom(ori => ori.NewsId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(ori => ori.CategoryId))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(ori => ori.SubCategoryId))
                .ForMember(dest => dest.ExtraAppId, opt => opt.MapFrom(ori => ori.ExtraAppId))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(ori => ori.Site))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(ori => ori.Title))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(ori => ori.Summary))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(ori => ori.Content))
                .ForMember(dest => dest.AppOpenURL, opt => opt.MapFrom(ori => ori.AppOpenURL))
                .ForMember(dest => dest.PublishTime, opt => opt.MapFrom(ori => ori.PublishTime))
                .ForMember(dest => dest.Stamp, opt => opt.MapFrom(ori => ori.PublishTime.UnixStamp()))
                .IgnoreAllNonExisting();

            #endregion
        }
    }
}
