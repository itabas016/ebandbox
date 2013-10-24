using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FrameMobile.Model;
using FrameMobile.Model.Account;

namespace FrameMobile.Domain
{
    public class AccountMapping
    {
        internal static void CreateMap()
        {
            Mapper.CreateMap<RegisterView, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(ori => ori.Name.ToLower()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(ori => ori.Password.GetMD5Hash()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(ori => ori.Email))
                .ForMember(dest => dest.SecurityQuestion, opt => opt.MapFrom(ori => ori.SecurityQuestion))
                .ForMember(dest => dest.SecurityAnswer, opt => opt.MapFrom(ori => ori.SecurityAnswer))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(ori => ori.Address))
                .ForMember(dest => dest.QQ, opt => opt.MapFrom(ori => ori.QQ))
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(ori => ori.PostCode))
                .ForMember(dest => dest.Tel, opt => opt.MapFrom(ori => ori.Tel))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .IgnoreAllNonExisting();
        }
    }
}
