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
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(ori => ori.Comment))
                .IgnoreAllNonExisting();
        }
    }
}
