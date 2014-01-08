﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Cache;
using FrameMobile.Core;
using FrameMobile.Domain.Service;
using FrameMobile.Web;
using StructureMap.Configuration.DSL;

namespace FrameMobile.Domain
{
    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            RegistyCommonService();

            RegistyNewsService();

            RegistyThemeService();
        }

        private void RegistyCommonService()
        {
            For<IRequestRepository>().Use<RequestRepository>();
            For<INewsDbContextService>().Use<NewsDbContextService>();
            For<IThemeDbContextService>().Use<ThemeDbContextService>();
            For<ICacheManagerHelper>().Use<RedisCacheHelper>();
            For<IAccountService>().Use<AccountService>();
            For<ICookieService>().Use<CookieService>();
            For<IMobileUIService>().Use<MobileUIService>();
        }

        private void RegistyNewsService()
        {
            For<INewsUIService>().Use<NewsUIService>();

#if DEBUG
            For<INewsService>().Use<NewsFakeService>();
#else
            For<INewsService>().Use<NewsService>();
#endif
        }

        private void RegistyThemeService()
        {
            For<IWallPaperUIService>().Use<WallPaperUIService>();

#if DEBUG
            For<IWallPaperService>().Use<WallPaperFakeService>();
#else
            For<IWallPaperService>().Use<WallPaperService>();
#endif
        }
    }
}
