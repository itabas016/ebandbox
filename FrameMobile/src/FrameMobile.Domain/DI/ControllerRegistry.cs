using System;
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

            RegistyHelperService();

            RegistyNewsService();

            RegistyThemeService();
        }

        private void RegistyCommonService()
        {
            For<IRequestRepository>().Use<RequestRepository>();
            For<ICommonDbContextService>().Use<CommonDbContextService>();
            For<INewsDbContextService>().Use<NewsDbContextService>();
            For<IThemeDbContextService>().Use<ThemeDbContextService>();
            For<ICacheManagerHelper>().Use<RedisCacheHelper>();
            For<ICacheService>().Use<CacheService>();
            For<IRedisCacheService>().Use<RedisCacheService>();
            For<IAccountService>().Use<AccountService>();
            For<ICookieService>().Use<CookieService>();
            For<IMobileUIService>().Use<MobileUIService>();
            For<IRadarService>().Use<RadarService>();
        }

        private void RegistyHelperService()
        {
            For<ICommonServiceHelper>().Use<CommonServiceHelper>();
            For<INewsServiceHelper>().Use<NewsServiceHelper>();
            For<IWallPaperServiceHelper>().Use<WallPaperServiceHelper>();
        }

        private void RegistyNewsService()
        {
            For<INewsUIService>().Use<NewsUIService>();
            For<INewsRedisCacheService>().Use<NewsRedisCacheService>();

#if DEBUG
            For<INewsService>().Use<NewsFakeService>();
#else
            For<INewsService>().Use<NewsService>();
#endif
        }

        private void RegistyThemeService()
        {
            For<IWallPaperUIService>().Use<WallPaperUIService>();
            For<IThemeRedisCacheService>().Use<ThemeRedisCacheService>();

#if DEBUG
            For<IWallPaperService>().Use<WallPaperService>();
#else
            For<IWallPaperService>().Use<WallPaperService>();
#endif
        }
    }
}
